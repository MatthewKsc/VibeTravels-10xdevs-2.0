import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe } from '@angular/common';
import { Note } from '../../models/note.model';

@Component({
  selector: 'app-note-details-dialog',
  imports: [MatDialogModule, MatButtonModule, DatePipe],
  templateUrl: './note-details-dialog.html',
  styleUrl: './note-details-dialog.scss'
})
export class NoteDetailsDialog {
  private dialogRef = inject(MatDialogRef<NoteDetailsDialog>);
  note = inject<Note>(MAT_DIALOG_DATA);

  close(): void {
    this.dialogRef.close();
  }
}
