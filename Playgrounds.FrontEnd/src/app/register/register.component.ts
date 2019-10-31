import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Member } from '../_models/Member';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  member: Member;
  registerForm: FormGroup;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      loginname: new FormControl('', Validators.required),
      emailaddress: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4)]),
      confirmPassword: new FormControl('', Validators.required)
    }, this.passwordMatchValidator);
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }

  register() {
    if (this.registerForm.valid) {
      this.member = Object.assign({}, this.registerForm.value);
      this.authService.register(this.member).subscribe(() => {
        this.authService.login(this.member).subscribe(() => {
          this.router.navigate(['/member/photos-manager']);
        });
      }, error => {
          console.log(error);
      });
    }
  }
}
