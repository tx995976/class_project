import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-panelres',
  templateUrl: './panelres.component.html',
  styleUrls: ['./panelres.component.less']
})

export class PanelresComponent {

  url_wss: string = "wss://localhost:7232/wss/dashboard"
  ws: WebSocket | undefined

  flag_groupinit: boolean = false;
  flag_subinit: boolean = false;
  flag_question: boolean = false;
  flag_contest: boolean = false;

  subservers : group_info[] = [];

  //from master
  dashboard: string | undefined
  group_num: number = 0;
  each_group: number = 0;

  level : number  = 0;
  each_level_case : number  = 0;
  case_questions: number  = 0;

  constructor() { }


  ngOnInit() {
    this.init_dashboard();
  }

  init_dashboard() {
    //open wss
    console.log("open wss dashboard");
    
    this.ws = new WebSocket(this.url_wss);
    this.ws.addEventListener('message', (e) => {
      this.received_data(e.data);
    })
  }

  received_data(data: any) {
    const [action, para1, para2] = `${data}`.split('|');
    if (action === 'score') {
      this.dashboard = para1;
    }
    else if (action === 'group') {
      this.group_num = Number.parseInt(para1);
      this.each_group = Number.parseInt(para2);

      for(let i = 0; i < this.group_num; i++) 
        for(let j = 0; j < this.each_group;j++){
          this.subservers.push({
            group_num: i,
            seq_num : j
          });
        }
      this.flag_groupinit = true;
      this.flag_subinit = true;
    }
    else {
      alert("unknown action")
    }
  }

  init_questions() {
    var get_case = new Request(`https://localhost:7232/api/server/initcase?level=${this.level}&each_level_cases=${this.each_level_case}&each_cases=${this.case_questions}`,{
        mode: 'cors',
    });
    fetch(get_case)
    .then( e => e.text())
      .then(data => {
        alert(`init case: ${data}`)
      })
      this.flag_question = true;
    }

  start_contest() {
    var get_case = new Request(`https://localhost:7232/api/server/contest?status=true`,{
        mode: 'cors',
    });
    fetch(get_case)
    this.flag_contest = true;
  }
  
  stop_contest() {
    var get_case = new Request(`https://localhost:7232/api/server/contest?status=false`,{
      mode: 'cors',
    });
    fetch(get_case)
    this.flag_contest = true;
  }

}

export interface group_info {
  group_num: number;
  seq_num: number;
}