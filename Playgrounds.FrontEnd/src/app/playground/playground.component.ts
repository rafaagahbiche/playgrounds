import { Component, OnInit } from '@angular/core';
import { PlaygroundsService } from '../_services/playgrounds.service';
import { Playground } from '../_models/Playground';
import { AuthService } from '../_services/auth.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-playground',
  templateUrl: './playground.component.html',
  styleUrls: ['./playground.component.scss']
})
export class PlaygroundComponent implements OnInit {

  constructor(private playgroundsService: PlaygroundsService, private authService: AuthService, private route: ActivatedRoute) { }
  playground: Playground;
  bsConfig: Partial<BsDatepickerConfig>;
  checkInForm: FormGroup;

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.playground = data['playground'];
    });

    this.checkInForm = new FormGroup({
      checkInDate: new FormControl('', Validators.required)
    });
    
    this.bsConfig = {
      containerClass: 'theme-red'
    };
  }

  checkInMemberToPlayground() {
    if (this.checkInForm.valid) {
      var checkInModel = Object.assign({}, this.checkInForm.value);
      checkInModel.playgroundId = this.playground.id;
      checkInModel.memberId = 2;
      this.playgroundsService.checkInToPlayground(checkInModel).subscribe(response => {
        console.log(response);
      });
    }
  }
}
