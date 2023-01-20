package main.spi.Imethod;

import java.util.Iterator;
import java.util.ServiceLoader;

public class App{
    public static void main(String[] args) {
        ServiceLoader<method> loader = ServiceLoader.load(method.class);
        System.out.println("starting loader");
        Iterator<method> iter = loader.iterator();
        while (iter.hasNext()){
            var reg = iter.next();
            System.out.println("class: " + reg.getClass().getName());
            reg.show_version();
        }
    }
}