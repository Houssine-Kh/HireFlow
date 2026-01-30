import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { Tooltip } from 'primeng/tooltip';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterOutlet,
    CommonModule,
    RouterModule,
    Tooltip
  ],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.scss',
})
export class AdminLayout {

}
