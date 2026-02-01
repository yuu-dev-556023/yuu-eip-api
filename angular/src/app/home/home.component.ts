import {AuthService, LocalizationPipe} from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import {CommonModule, NgTemplateOutlet} from "@angular/common";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [NgTemplateOutlet, LocalizationPipe, CommonModule]
})
export class HomeComponent {
  private authService = inject(AuthService);

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  login() {
    this.authService.navigateToLogin();
  }
}
