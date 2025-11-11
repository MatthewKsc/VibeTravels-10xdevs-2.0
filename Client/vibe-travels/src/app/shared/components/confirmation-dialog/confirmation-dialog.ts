import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { Component, inject } from '@angular/core';

import { ConfirmationDialogData } from '../../models/confirmation-dialog.model';

@Component({
  selector: 'app-confirmation-dialog',
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <div class="confirmation-dialog-container">
        <h2 mat-dialog-title>{{ data.title }}</h2>
        
        <mat-dialog-content>
          <p>{{ data.message }}</p>
        </mat-dialog-content>
    
        <mat-dialog-actions align="end">
          <button mat-button (click)="onCancel()">
            {{ data.cancelText || 'Cancel' }}
          </button>
          <button mat-raised-button color="warn" (click)="onConfirm()">
            {{ data.confirmText || 'Confirm' }}
          </button>
        </mat-dialog-actions>
    </div>
  `,
  styles: [`
    .confirmation-dialog-container {
        background-color: var(--system-secondary-background);
    }

    p {
      margin: 0;
      color: var(--mat-sys-on-surface);
    }

    mat-dialog-actions {
      gap: 0.5rem;
    }
  `]
})
export class ConfirmationDialog {
  private dialogRef = inject(MatDialogRef<ConfirmationDialog>);
  data = inject<ConfirmationDialogData>(MAT_DIALOG_DATA);

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
