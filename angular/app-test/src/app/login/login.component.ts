import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.less']
})

export class LoginComponent implements OnInit {

    User: Userinfo = {
        id: '',
        password: '',
    };

    constructor(
        private router: Router
    ) {}

    ngOnInit(): void {

    }

    action_login() {
        var header = new Headers();
        var httprq = new Request(`https://localhost:7232/api/login?id=${this.User.id}&password=${this.User.password}`,
        {
            mode: 'cors',
        });
        fetch(httprq)
            .then(res => {
                alert("ok");
                this.router.navigateByUrl('/panel');
            })
    }
}


export interface Userinfo {
    id: string;
    password: string;
}
