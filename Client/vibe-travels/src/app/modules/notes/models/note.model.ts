export interface Note {
  id: string;
  title: string;
  location: string;
  content: string;
  createdAt: Date;
  updatedAt?: Date;
}

export interface NoteFormDialogData {
  note?: Note;
  mode: 'add' | 'edit';
}

export interface NoteFormData {
  title: string;
  location: string;
  content: string;
}