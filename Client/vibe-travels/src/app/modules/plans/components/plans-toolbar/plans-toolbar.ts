import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-plans-toolbar',
  imports: [MatToolbarModule, MatIconModule],
  templateUrl: './plans-toolbar.html',
  styleUrl: './plans-toolbar.scss'
})
export class PlansToolbar {
}
