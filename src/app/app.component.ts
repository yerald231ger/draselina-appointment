import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppointmentCalendarComponent } from './appointment-calendar/appointment-calendar.component';
import { UserFormComponent } from './user-form/user-form.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, AppointmentCalendarComponent, UserFormComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'wpa-appointment-app';
}
