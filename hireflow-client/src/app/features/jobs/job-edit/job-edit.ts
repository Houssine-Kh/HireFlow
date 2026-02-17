  import { Component, inject, OnInit, effect } from '@angular/core';
  import { CommonModule } from '@angular/common';
  import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
  import { ActivatedRoute, Router } from '@angular/router';

  // PrimeNG
  import { ButtonModule } from 'primeng/button';
  import { InputTextModule } from 'primeng/inputtext';
  import { TextareaModule } from 'primeng/textarea';
  import { SelectModule } from 'primeng/select';
  import { CardModule } from 'primeng/card';
  import { TagModule } from 'primeng/tag';
  import { ToastModule } from 'primeng/toast';
  import { ConfirmDialogModule } from 'primeng/confirmdialog'; // 👈 Added
  import { FluidModule } from 'primeng/fluid';

  import { JobStore } from '../../../infrastructure/jobs/job.store';
  import { JobDto } from '../../../domain/dtos/job.dto';
  import { FeedbackService } from '../../../core/services/feedback.service'; // 👈 Added

  @Component({
    selector: 'app-job-edit',
    standalone: true,
    imports: [
      CommonModule, ReactiveFormsModule,
      ButtonModule, InputTextModule, TextareaModule,
      SelectModule, CardModule, TagModule, 
      ToastModule, ConfirmDialogModule, FluidModule // 👈 Added ConfirmDialogModule
    ],
    templateUrl: './job-edit.html',
  })
  export class JobEditComponent implements OnInit {
    
    readonly store = inject(JobStore);
    private readonly feedback = inject(FeedbackService); // 👈 Injected FeedbackService
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private fb = inject(FormBuilder);

    jobId: string | null = null;
    job: JobDto | undefined;
    
    form: FormGroup;
    
    workModes = [
      { label: 'Remote', value: 'Remote' },
      { label: 'On-Site', value: 'OnSite' },
      { label: 'Hybrid', value: 'Hybrid' }
    ];

    constructor() {
      this.form = this.fb.group({
        title: ['', Validators.required],
        workMode: [null],
        description: ['', [Validators.maxLength(4000)]]
      });
    }

    ngOnInit() {
      this.jobId = this.route.snapshot.paramMap.get('id');
      
      if (this.store.jobs().length === 0) {
          this.store.loadJobs(); 
      }
    }

    loadEffect = effect(() => {
      const jobs = this.store.jobs();
      if (this.jobId && jobs.length > 0) {
          this.job = jobs.find(j => j.id === this.jobId);
          if (this.job && !this.form.dirty) {
              this.form.patchValue({
                  title: this.job.title,
                  workMode: this.job.workMode,
                  description: this.job.description
              });
          }
      }
    });

    // Watch for Store Errors
    errorEffect = effect(() => {
      const error = this.store.error()
      if(error){
        this.feedback.error(error);
      }

      const action = this.store.lastAction(); 

      if (action === 'update') {
          this.feedback.success('Changes saved successfully.', 'Saved');
          this.form.markAsPristine(); //  Only mark pristine AFTER server confirms
      }

      if (action === 'publish') {
          this.feedback.success('Job is now live!', 'Published');
          this.router.navigate(['/jobs']); // Only navigate AFTER server confirms
      }

      if (action === 'close') {
          this.feedback.info('Job has been closed.', 'Closed');
          this.router.navigate(['/jobs']);
      }
  });

    onSave() {
      if (this.form.invalid || !this.jobId) return;

      this.store.updateJob({
          id: this.jobId,
          request: this.form.getRawValue()
      });
    }

    onPublish() {
      if (!this.jobId) return;
      this.confirmAction('publish');
    }

    onClose() {
      if (!this.jobId) return;
      this.confirmAction('close');
    }

    confirmAction(action: 'publish' | 'close') {
      let message = '';
      let header = '';
      let icon = '';
      let onAccept = () => {};

      switch (action) {
        case 'publish':
          message = 'Are you sure you want to <b>Publish</b> this job? It will be visible to all candidates.';
          header = 'Confirm Publication';
          icon = 'pi pi-send';
          onAccept = () => {
            // Auto-save if dirty before publishing
            if (this.form.dirty) this.onSave();
            
            this.store.publishJob(this.jobId!);
          };
          break;

        case 'close':
          message = 'Are you sure you want to <b>Close</b> this job? You will stop receiving new applications.';
          header = 'Close Job';
          icon = 'pi pi-ban';
          onAccept = () => {
              this.store.closeJob(this.jobId!);
          };
          break;
      }

      // Trigger the dialog
      this.feedback.confirm(message, onAccept, header, icon);
    }

    goBack() {
      this.router.navigate(['/jobs']);
    }

    hasError(field: string): boolean {
      const control = this.form.get(field);
      // Combine Client-side Angular errors AND Server-side Store errors
      return (!!control?.invalid && !!control?.touched) || !!this.store.validationErrors()[field];
    }

    getSeverity(status: string) {
      switch (status) {
        case 'Published': return 'success';
        case 'Draft': return 'secondary';
        case 'Closed': return 'danger';
        default: return 'info';
      }
    }
  }