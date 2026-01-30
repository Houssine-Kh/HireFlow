import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// PrimeNG Imports
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag'; // For the "Pending" badges
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MenuItem, MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';


@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule, FormsModule,
    TableModule, ButtonModule, TagModule, 
    InputTextModule, IconFieldModule, InputIconModule,
    TooltipModule, ConfirmDialogModule,MenuModule
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './user-management.html',
})
export class UserManagement implements OnInit {

  // This will eventually come from your AdminStore
  users = [
    { id: '1', name: 'Houssine Khafif', email: 'houssine@test.com', role: 'Candidate', status: 'Active' },
    { id: '2', name: 'John Recruiter', email: 'john@company.com', role: 'Recruiter', status: 'Pending' }, // 👈 Needs Approval
    { id: '3', name: 'Jane HR', email: 'jane@tech.com', role: 'Recruiter', status: 'Active' },
    { id: '4', name: 'Admin User', email: 'admin@hireflow.com', role: 'Admin', status: 'Active' },
  ];

items: MenuItem[] | undefined;
  selectedUser: any;

  constructor(private confirmationService: ConfirmationService, private messageService: MessageService) {}

ngOnInit() {
    this.items = [
        {
            label: 'Options',
            items: [
                {
                    label: 'Edit',
                    icon: 'pi pi-pencil',
                    command: () => this.editUser(this.selectedUser)
                },
                {
                    label: 'Reset Password',
                    icon: 'pi pi-key',
                },
                {
                    label: 'Deactivate',
                    icon: 'pi pi-ban',
                    styleClass: 'text-red-600' // Make it look dangerous
                }
            ]
        }
    ];
  }

  getSeverity(status: string) {
    switch (status) {
      case 'Active': return 'success'; // Green
      case 'Pending': return 'warn';   // Orange
      case 'Banned': return 'danger';  // Red
      default: return 'info';
    }
  }

setSelectedUser(user: any) {
    this.selectedUser = user;
  }
  
  editUser(user: any) {
      console.log('Editing', user.name);
  }

  approveRecruiter(user: any) {
    this.confirmationService.confirm({
      message: `Are you sure you want to approve <b>${user.name}</b> as a Recruiter?`,
      header: 'Confirm Approval',
      icon: 'pi pi-check-circle',
      acceptButtonStyleClass: 'p-button-success p-button-text',
      rejectButtonStyleClass: 'p-button-text p-button-plain',
      accept: () => {
        // TODO: Call your Store here: this.store.approveRecruiter(user.id);
        
        // Optimistic update for demo
        user.status = 'Active'; 
        this.messageService.add({ severity: 'success', summary: 'Approved', detail: `${user.name} is now active.` });
      }
    });
  }
}