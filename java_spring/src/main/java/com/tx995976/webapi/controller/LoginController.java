package com.tx995976.webapi.controller;

import com.tx995976.webapi.Service.ThreadService;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.web.bind.annotation.*;

import javax.annotation.Resource;

@RestController
@Api("用户登录")
@CrossOrigin
@RequestMapping("/user")
public class LoginController {
    @ApiOperation("用户登录")
    @PostMapping("/login")
    public String login(@RequestParam("username") String username,@RequestParam("password") String password){
        return "ok";
    }

    @ApiOperation("用户注册")
    @PostMapping("/register")
    public String register(@RequestParam("username") String username,@RequestParam("password") String password){
        return "ok";
    }

    @ApiOperation("找回密码")
    @GetMapping("/findpassword")
    public String find(@RequestParam("username") String username){
        return "ok";
    }

    @ApiOperation("多线程测试")
    @GetMapping("/Mocktest")
    public String Mockthread(){
        ThreadService instance = new ThreadService();
        instance.Mock();
        return "ok";
    }
}
