import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Component, computed, DestroyRef, inject, signal, WritableSignal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { finalize } from 'rxjs';

import { NoteFormData, NoteFormDialogData } from '../../models/note.model';
import { NotesApiService } from '../../services/notes-api.service';
import { NotificationService } from '../../../../shared/services';

@Component({
  selector: 'app-note-form-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './note-form-dialog.html',
  styleUrl: './note-form-dialog.scss'
})
export class NoteFormDialog {
  private dialogRef = inject(MatDialogRef<NoteFormDialog>);
  private formBuilder = inject(FormBuilder);
  private apiService = inject(NotesApiService);
  private destroyRef = inject(DestroyRef);
  private notificationService = inject(NotificationService);
  
  data: NoteFormDialogData = inject<NoteFormDialogData>(MAT_DIALOG_DATA);
  processingRequest: WritableSignal<boolean> = signal<boolean>(false);

  noteForm = this.formBuilder.group({
    title: [
      this.data.note?.title || '',
      [Validators.required, Validators.minLength(1), Validators.maxLength(200)]
    ],
    location: [
      this.data.note?.location || '',
      [Validators.required, Validators.minLength(1), Validators.maxLength(255)]
    ],
    content: [
      this.data.note?.content || '',
      [Validators.required, Validators.minLength(100), Validators.maxLength(10000)]
    ]
  });

  private contentValue = toSignal(this.noteForm.get('content')!.valueChanges, { initialValue: this.data.note?.content || '' });

  contentLength = computed(() => this.contentValue()?.length || 0);
  isAddMode = computed(() => this.data.mode === 'add');
  dialogTitle = computed(() => this.isAddMode() ? 'Add New Note' : 'Edit Note');

  get title() { return this.noteForm.get('title'); }
  get location() { return this.noteForm.get('location'); }
  get content() { return this.noteForm.get('content'); }

  cancel = (): void =>this.dialogRef.close(false);

  save(): void {
    if (this.noteForm.invalid) {
      this.noteForm.markAllAsTouched();
      return;
    }

    this.processingRequest.set(true);
    const formValue = this.noteForm.value;

    const noteFormData: NoteFormData = {
      title: formValue.title!,
      location: formValue.location!,
      content: formValue.content!
    }

    const operation$ = this.isAddMode()
      ? this.apiService.addNote(noteFormData)
      : this.apiService.updateNote(this.data.note!.id, noteFormData);

    operation$
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.processingRequest.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess(this.getSuccessMessage());
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error saving note:', error);
          this.notificationService.notifyError(this.getErrorMessage());
        }
      });
  }

  private getSuccessMessage(): string {
    return this.isAddMode() 
      ? 'Note added successfully!' 
      : 'Note updated successfully!';
  }

  private getErrorMessage(): string {
    return this.isAddMode()
      ? 'Failed to add note. Please try again.'
      : 'Failed to update note. Please try again.';
  }
}
