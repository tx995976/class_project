package com.tx995976.webapi.Service;

import org.springframework.stereotype.Service;

import javax.websocket.OnMessage;
import javax.websocket.server.ServerEndpoint;

@Service
@ServerEndpoint("/api/websocket")
public class WebSocketService {
    @OnMessage
    public void onMessage(String message) {
        switch (message){
            case "success":
                System.out.println("success");
                break;
            case "fail":
                System.out.println("failed");
                break;
            case "error":
                System.out.println("error");
                break;
            default:
                System.out.println("unknown");
                break;
        }
    }
}
