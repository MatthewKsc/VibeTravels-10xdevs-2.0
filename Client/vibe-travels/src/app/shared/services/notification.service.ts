import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private snackBar = inject(MatSnackBar);

  private readonly defaultDuration = 4000;
  private readonly defaultDismissText = 'Close';

  notifySuccess(message: string, dismissText?: string): void {
    const config: MatSnackBarConfig = {
      duration: this.defaultDuration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-success']
    };

    this.snackBar.open(message, dismissText || this.defaultDismissText, config);
  }

  notifyError(message: string, dismissText?: string): void {
    const config: MatSnackBarConfig = {
      duration: this.defaultDuration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-error']
    };

    this.snackBar.open(message, dismissText || this.defaultDismissText, config);
  }

  notifyWarning(message: string, dismissText?: string): void {
    const config: MatSnackBarConfig = {
      duration: this.defaultDuration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-warning']
    };

    this.snackBar.open(message, dismissText || this.defaultDismissText, config);
  }
}
