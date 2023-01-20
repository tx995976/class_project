import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-resview',
    templateUrl: './resview.component.html',
    styleUrls: ['./resview.component.less']
})

export class ResviewComponent {

    url_ws: string = 'wss://localhost:7232/wss';
    ws: WebSocket | undefined;

    array_res: info_res[] = [];
    accept_num: number = 0;
    accept_ratio: string = '0.0%';
    num_info_max : number = 15;
    time_query : number = 1000;

    timer: NodeJS.Timer | undefined;

    constructor() { }

    action_openws() {
        this.ws = new WebSocket(this.url_ws);
        this.ws.addEventListener('open', () => {
            console.log('socket opened');
        })
        this.ws.addEventListener('message', (event) => {
            console.log(event.data);
            this.add_new_info(event.data);
        })
        this.ws.addEventListener('error',() => {
            alert('failed to open socket')
        });
        this.timer = setInterval(() => { this.test_send() }, this.time_query);

    }

    action_closews() {
        this.ws!.send('ws_close');
        this.ws = undefined;
        console.log('ws closed')

        clearInterval(this.timer);
        this.timer = undefined;
    }

    add_new_info(data: any) {
        const [ques, answer, result] = `${data}`.split('|');
        if (this.array_res.length >= this.num_info_max) {
            let res = this.array_res.shift()?.results;
            if(res === 'accept')
                this.update_ratio(-1);
            else
                this.update_ratio(0);
        }

        this.array_res.push({
            question: ques,
            answer: answer,
            results: result
        });
        if (result === 'accept')
            this.update_ratio(1);
        else
            this.update_ratio(0);
  }

    test_send() {
        this.ws!.send("get");
    }

    update_ratio(dir: number) {
        this.accept_num += dir;
        let ratio = this.accept_num / this.array_res.length * 100;
        this.accept_ratio = `${ratio.toFixed(1)}%`;
    }
}


export interface info_res {
    question: string,
    answer: string,
    results: string
}