import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

// PrimeNG Imports
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { TooltipModule } from 'primeng/tooltip';
import { MenuModule } from 'primeng/menu';
import { ToastModule } from 'primeng/toast';
import { MenuItem } from 'primeng/api';
  import { JobDto } from '../../../domain/dtos/job.dto';
import { JobStore } from '../../../infrastructure/jobs/job.store';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';


@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule,
    TableModule, ButtonModule, TagModule,
    InputTextModule, IconFieldModule, InputIconModule,
    TooltipModule, MenuModule, ToastModule, DialogModule, SelectModule
  ],
  templateUrl: './job-list.html',
})
export class JobListComponent implements OnInit {

  readonly store = inject(JobStore);
  private readonly router = inject(Router);
  private fb = inject(FormBuilder);


  items: MenuItem[] | undefined;
  selectedJob: JobDto | null = null;


  // 🆕 Dialog State
  visible = false;
  createJobForm: FormGroup;

  // 🆕 Dropdown Options
  workModes = [
    { label: 'Remote', value: 'Remote' },
    { label: 'On-Site', value: 'OnSite' }, // Value matches Backend Enum
    { label: 'Hybrid', value: 'Hybrid' }
  ];

  constructor() {
    this.createJobForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      workMode: [null] // Optional for Drafts
    });
  }

  ngOnInit() {
    this.store.loadJobs();
  }

  getSeverity(status: string) {
    switch (status) {
      case 'Published': return 'success'; // Green
      case 'Draft': return 'secondary';   // Gray/Slate
      case 'Closed': return 'danger';     // Red
      default: return 'info';
    }
  }

  getWorkModeSeverity(mode: string) {
    switch (mode) {
      case 'Remote': return 'success';
      case 'Hybrid': return 'info';
      case 'OnSite': return 'warn';
      default: return 'secondary';
    }
  }

  // Placeholder for future actions
  setSelectedJob(job: JobDto) {
    this.selectedJob = job;
    this.items = [
      {
        label: 'Actions',
        items: [
          {
            label: 'Edit Job',
            icon: 'pi pi-pencil',
            command: () => {
              this.router.navigate(['/jobs', job.id])
            }
          },
          job.status === 'Published' ? {
            label: 'Close Job',
            icon: 'pi pi-ban',
            styleClass: 'text-red-600',
            command: () => console.log('Close', job.id)
          } : {
            label: 'Publish Job',
            icon: 'pi pi-send',
            styleClass: 'text-green-600',
            command: () => console.log('Publish', job.id)
          }
        ]
      }
    ];
  }
showCreateDialog() {
    this.createJobForm.reset();
    this.visible = true;
  }
onCreateSubmit() {
    if (this.createJobForm.invalid) return;

    const formValue = this.createJobForm.getRawValue();

    this.store.createJob({
            title: formValue.title,
            workMode: formValue.workMode,
            description: '' 
        });
  
    this.visible = false;
  }
}