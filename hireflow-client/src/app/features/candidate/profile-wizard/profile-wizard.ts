import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

// PrimeNG Imports (Ensure you have primeng installed)
import { StepperModule } from 'primeng/stepper';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { FileUploadModule } from 'primeng/fileupload';
import { MessageModule } from 'primeng/message';
import { CardModule } from 'primeng/card';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CandidateService } from '../../../infrastructure/candidates/candidate.service';
import { EducationLevel } from '../../../domain/dtos/candidate.dto';
import { CandidateStore } from '../../../infrastructure/candidates/candidate.store';

@Component({
  selector: 'app-profile-wizard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    StepperModule, // Handles p-stepper, p-step-list, p-step, p-step-panels, p-step-panel
    ButtonModule,
    InputTextModule,
    SelectModule,
    FileUploadModule,
    MessageModule,
    CardModule
  ],
  templateUrl: './profile-wizard.html',
  styleUrls: ['./profile-wizard.scss']
})
export class ProfileWizardComponent {
  readonly store = inject(CandidateStore);
  private fb = inject(FormBuilder);

  readonly isSubmitting = this.store.isLoading;

  activeStep: number = 1;

  educationLevels = [
    { label: 'High School', value: EducationLevel.Bac },
    { label: 'Technician / BTS / DUT', value: EducationLevel.BacPlus2 },
    { label: 'Bachelor\'s Degree', value: EducationLevel.BacPlus3 },
    { label: 'Master 1', value: EducationLevel.BacPlus4 },
    { label: 'Master 2 / Engineering', value: EducationLevel.BacPlus5 },
    { label: 'PhD', value: EducationLevel.PhD }
  ];

  profileForm = this.fb.group({
    phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9+\\- ]{8,15}$')]],
    linkedInUrl: ['', [Validators.pattern('https://.*linkedin.*')]],
    educationLevel: ['', Validators.required],
    resumeUrl: ['', Validators.required]
  });


  onFileUpload(event: any) {
    const file = event.files[0];
    setTimeout(() => {
      const mockUrl = `https://storage.hireflow.com/cvs/${file.name}`;
      this.profileForm.patchValue({ resumeUrl: mockUrl });
    }, 1000);
  }

  submitProfile() {
    if (this.profileForm.invalid) return;

    const formValue = this.profileForm.getRawValue();

    // transform the form values to match the interface 
    const profileData = {
      phoneNumber: formValue.phoneNumber!,
      linkedInUrl: formValue.linkedInUrl || undefined,
      educationLevel: formValue.educationLevel as EducationLevel,
      resumeUrl: formValue.resumeUrl!
    };


    this.store.createProfile(profileData);
  }
}