import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavigationRoot } from './modules/navigation/components/navigation-root/navigation-root';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavigationRoot],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  readonly title = signal('vibe-travels');
}
