import { Component, inject, OnInit } from '@angular/core';
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
import { AdminStore } from '../../../infrastructure/admin/admin.store';
import { UserDto } from '../../../domain/dtos/admin.dto';
import { FeedbackService } from '../../../core/services/feedback.service';
import { ToastModule } from 'primeng/toast';





@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule, FormsModule,
    TableModule, ButtonModule, TagModule, 
    InputTextModule, IconFieldModule, InputIconModule,
    TooltipModule, ConfirmDialogModule,MenuModule,ToastModule
  ],
  templateUrl: './user-management.html',
})
export class UserManagement implements OnInit {

  readonly store = inject(AdminStore);
  private readonly feedback = inject(FeedbackService);


  items: MenuItem[] | undefined;
    selectedUser: UserDto | null = null;


  ngOnInit() {
      this.store.loadUsers();
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
      this.updateMenuOptions(user);  }
      
  updateMenuOptions(user: UserDto) {
      this.items = [
        {
          label: 'Options',
          items: [
            {
              label: 'Edit Profile',
              icon: 'pi pi-pencil',
              command: () => console.log('Edit', user.email)
            },
            {
              label: 'Reset Password',
              icon: 'pi pi-key',
            },
            user.status === 'Banned' 
            ? {
                label: 'Unlock Account',
                icon: 'pi pi-lock-open',
                styleClass: 'text-green-600',
                command: () => this.confirmAction(user, 'unlock')
              }
            : {
                label: 'Ban Account',
                icon: 'pi pi-ban',
                styleClass: 'text-red-600',
                command: () => this.confirmAction(user, 'ban')
              }
          ]
        }
      ];
    }

  confirmAction(user: UserDto, action: 'approve' | 'ban' | 'unlock' | 'reject') {
      let message = '';
      let header = '';
      let onAccept = () => {};

      switch (action) {
        case 'approve':
          message = `Approve <b>${user.firstName}</b> as a Recruiter?`;
          header = 'Confirm Approval';
          onAccept = () => {
            this.store.approveRecruiter(user.id);
            this.feedback.success(`${user.firstName} is now active.`, 'Approved');
          };
          break;

        case 'reject':
          message = `Are you sure you want to <b>REJECT</b> the application for <b>${user.firstName}</b>?`;
          header = 'Reject Application';
          onAccept = () => {
            this.store.rejectRecruiter(user.id);
            this.feedback.info(`Application for ${user.firstName} was rejected.`);
          };
          break;

        case 'ban':
          message = `Are you sure you want to <b>BAN</b> ${user.firstName}?`;
          header = 'Suspend Account';
          onAccept = () => {
            this.store.banUser(user.id);
            // Using 'info' or 'success' here because the Admin action succeeded
            this.feedback.success('User account has been locked.', 'Banned');
          };
          break;

        case 'unlock':
          message = `Restore access for <b>${user.firstName}</b>?`;
          header = 'Restore Access';
          onAccept = () => {
            this.store.unlockUser(user.id);
            this.feedback.success('User access restored.', 'Unlocked');
          };
          break;
      }
      // Call the wrapper function
      this.feedback.confirm(message, onAccept, header);
    }


}