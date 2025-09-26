import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

interface Appointment {
  date: string;
  time: string;
  isBooked: boolean;
}

@Component({
  selector: 'app-appointment-calendar',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './appointment-calendar.component.html',
  styleUrl: './appointment-calendar.component.scss'
})
export class AppointmentCalendarComponent {
  appointmentForm: FormGroup;
  selectedDate: Date = new Date();
  currentMonth: Date = new Date();
  appointments: Appointment[] = [];
  timeSlots: string[] = [
    '09:00', '09:30', '10:00', '10:30', '11:00', '11:30',
    '12:00', '12:30', '13:00', '13:30', '14:00', '14:30',
    '15:00', '15:30', '16:00', '16:30', '17:00'
  ];

  constructor(private fb: FormBuilder) {
    this.appointmentForm = this.fb.group({
      date: ['', Validators.required],
      time: ['', Validators.required]
    });
    this.generateCalendarDays();
  }

  generateCalendarDays(): Date[] {
    const firstDay = new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth(), 1);
    const lastDay = new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth() + 1, 0);
    const daysInMonth = lastDay.getDate();
    const days: Date[] = [];

    for (let i = 1; i <= daysInMonth; i++) {
      days.push(new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth(), i));
    }
    return days;
  }

  get calendarDays(): Date[] {
    return this.generateCalendarDays();
  }

  selectDate(date: Date) {
    this.selectedDate = date;
    this.appointmentForm.patchValue({
      date: this.formatDate(date)
    });
  }

  formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  isDateSelected(date: Date): boolean {
    return this.formatDate(date) === this.formatDate(this.selectedDate);
  }

  isDatePast(date: Date): boolean {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    date.setHours(0, 0, 0, 0);
    return date < today;
  }

  previousMonth() {
    this.currentMonth = new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth() - 1, 1);
  }

  nextMonth() {
    this.currentMonth = new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth() + 1, 1);
  }

  getMonthYear(): string {
    return this.currentMonth.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
  }

  isTimeSlotBooked(time: string): boolean {
    const dateStr = this.formatDate(this.selectedDate);
    return this.appointments.some(apt => apt.date === dateStr && apt.time === time);
  }

  selectTime(time: string) {
    if (!this.isTimeSlotBooked(time)) {
      this.appointmentForm.patchValue({ time });
    }
  }

  onSubmit() {
    if (this.appointmentForm.valid) {
      const appointment: Appointment = {
        date: this.appointmentForm.value.date,
        time: this.appointmentForm.value.time,
        isBooked: true
      };
      this.appointments.push(appointment);
      console.log('Appointment scheduled:', appointment);
      this.appointmentForm.reset();
    }
  }
}
