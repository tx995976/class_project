package com.tx995976.webapi.Service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.stereotype.Service;

import java.util.concurrent.Executors;
import java.util.concurrent.LinkedBlockingDeque;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

@Service
@EnableAsync
public class ThreadService {
    int count = 0;
    ReentrantLock lock = new ReentrantLock(true);
    Condition condition = lock.newCondition();
    private class Producer implements Runnable {

        @Override
        public void run() {
            try {
                lock.lock();
                count++;
                System.out.println("add a goods | now：" + count);
                condition.signal();
                lock.unlock();
                Thread.sleep(500);
            }
            catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public class Consumer implements Runnable{
        @Override
        public void run(){
            lock.lock();
            if(count < 1){
                System.out.println("no goods");
                try {
                    condition.await();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            if(count > 0){
                System.out.println("get a goods | last:" + --count);
            }
            lock.unlock();
            try {
                Thread.sleep(500);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
    public void Mock(){

        ThreadPoolExecutor Poolprodu = new ThreadPoolExecutor(2,2,0L,TimeUnit.MILLISECONDS,new LinkedBlockingDeque<>(5));
        ThreadPoolExecutor Poolconsumer = new ThreadPoolExecutor(2,2,0L,TimeUnit.MILLISECONDS,new LinkedBlockingDeque<>(5));
        for(int i = 0; i < 5; i++){
            Poolprodu.execute(new Producer());
            Poolconsumer.execute(new Consumer());
        }
    }

}
