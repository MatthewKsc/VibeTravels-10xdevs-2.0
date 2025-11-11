import { Component, DestroyRef, inject, signal, WritableSignal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { NotesToolbar } from '../notes-toolbar/notes-toolbar';
import { NotesList } from '../notes-list/notes-list';
import { NotesApiService } from '../../services/notes-api.service';
import { NoteDetailsDialog } from '../note-details-dialog/note-details-dialog';
import { NoteFormDialog } from '../note-form-dialog/note-form-dialog';
import { Note, NoteFormDialogData } from '../../models/note.model';
import { ConfirmationDialog } from '../../../../shared/components';
import { ConfirmationDialogData } from '../../../../shared/models';
import { NotificationService } from '../../../../shared/services';

@Component({
  selector: 'app-notes-root',
  imports: [NotesToolbar, NotesList],
  templateUrl: './notes-root.html',
  styleUrl: './notes-root.scss'
})
export class NotesRoot {
  private apiService = inject(NotesApiService);
  private destroyRef = inject(DestroyRef);
  private dialog = inject(MatDialog);
  private notificationService = inject(NotificationService);

  notes: WritableSignal<Note[] | undefined> = signal<Note[] | undefined>(undefined);
  loadingData: WritableSignal<boolean> = signal<boolean>(false);

  constructor() {
    this.fetchNotes();
  }

  onAddNote(): void {
    const dialogRef = this.dialog.open<NoteFormDialog, NoteFormDialogData, boolean>(NoteFormDialog, {
      data: { mode: 'add' },
      width: '600px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchNotes();
      }
    });
  }

  onViewNote(noteId: string): void {
    const note: Note | undefined = this.getNoteById(noteId);

    if (!note) {
      this.notificationService.notifyWarning('Note not found');
      return;
    }

    this.dialog.open(NoteDetailsDialog, { data: note, width: '600px' });
  }

  onEditNote(noteId: string): void {
    const note: Note | undefined = this.getNoteById(noteId);

    if (!note) {
      this.notificationService.notifyWarning('Note not found');
      return;
    }

    const dialogRef = this.dialog.open<NoteFormDialog, NoteFormDialogData, boolean>(NoteFormDialog, {
      data: { note, mode: 'edit' },
      width: '600px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchNotes();
      }
    });
  }

  onDeleteNote(noteId: string): void {
    const dialogRef = this.dialog.open<ConfirmationDialog, ConfirmationDialogData, boolean>(
      ConfirmationDialog,
      {
        data: {
          title: 'Delete Note',
          message: 'Are you sure you want to delete this note? This action cannot be undone.',
          confirmText: 'Delete',
          cancelText: 'Cancel'
        },
        width: '400px'
      }
    );

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.deleteNote(noteId);
      }
    });
  }

  fetchNotes(): void {
    this.loadingData.set(true);

    this.apiService.getNotes()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.loadingData.set(false))
      )
      .subscribe({
        next: (notes: Note[]) => this.notes.set(notes),
        error: (error: any) => {
          console.error('Error fetching notes:', error);
          this.notificationService.notifyError('Failed to load notes. Please try again.');
        }
      });
  }

  deleteNote(noteId: string): void {
    this.apiService.deleteNote(noteId)
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Note deleted successfully!');
          this.fetchNotes();
        },
        error: (error) => {
          console.error('Error deleting note:', error);
          this.notificationService.notifyError('Failed to delete note. Please try again.');
        }
      });
  }

  private getNoteById = (noteId: string): Note | undefined => this.notes()?.find(n => n.id === noteId);
}
