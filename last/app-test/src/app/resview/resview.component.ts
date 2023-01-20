import { Component, OnInit ,Input, OnChanges, SimpleChanges} from '@angular/core';

@Component({
    selector: 'app-resview',
    templateUrl: './resview.component.html',
    styleUrls: ['./resview.component.less']
})

export class ResviewComponent {

    url_ws: string = 'wss://localhost:7232/wss/subinfo?';
    ws: WebSocket | undefined;

    array_res: info_res[] = [];
    
    servername: string = "等待节点加入"


    @Input() group_num : number = 0;
    @Input() seq_num : number = 0;

    @Input() level : number = 0;
    @Input() each_level_case : number = 0;

    @Input() flag_question: boolean = false;

    constructor() {}

    ngOnInit(){
        this.action_openws();
    }
    
    ngOnChanges(changes: SimpleChanges){
        if(this.flag_question)
            this.init_case();
    }

    action_openws() {
        this.ws = new WebSocket(this.url_ws+`group=${this.group_num}&seq_num=${this.seq_num}`);
        this.ws.addEventListener('open', () => {
            console.log('socket opened');
        })
        this.ws.addEventListener('message', (event) => {
            this.receive_data(event.data);
        })
        this.ws.addEventListener('error',() => {
            alert('failed to open socket')
        });
    }

    action_closews() {
        this.ws!.close();
        this.ws = undefined;
        console.log('ws closed')
    }

    receive_data(data: any) {
        const [action, para1, para2] = `${data}`.split('|');
        if(action === 'server'){
            this.servername = para1;
        }
        else if(action === 'result'){
            let case_id = Number.parseInt(para1);
            this.array_res[case_id].scores = Number.parseInt(para2);
            this.array_res[case_id].status = 'done';
        }
        else if(action === 'status'){
            let case_id = Number.parseInt(para1);
            this.array_res[case_id].status = para2;
        }
    }

    init_case(){
        var arr : info_res[] = [];
        for(let i = 0; i < this.level!; i++)
            for(let j = 0; j < this.each_level_case;j++){
                arr.push({
                    case_id: (i*this.each_level_case) + j,
                    level: i+1,
                    status: '',
                    scores: 0
                });
            }
        
        this.array_res = arr;
    }

    test_send() {
        this.ws!.send("get");
    }
}

export interface info_res {
    case_id: number,
    level: number,
    status: string,
    scores: Number
}