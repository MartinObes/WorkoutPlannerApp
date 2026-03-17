import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { EvaluationsResponse } from '../../models/evaluation.models';
import { EvaluationService } from '../../services/evaluation.service';

@Component({
  selector: 'app-evaluation-manager',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './evaluation-manager.html',
})
export class EvaluationManager implements OnInit {
  evaluations: EvaluationsResponse = { evaluations: [] };
  loading = false;
  error: string | null = null;
  constructor(
    private router: Router,
    private evaluationService: EvaluationService,
  ) {}

  ngOnInit(): void {
    //this.loadEvaluations();
  }

  LoadEvaluations(): void {
    this.loading = true;
    this.error = null;

    this.evaluationService.getByPlayerId;
  }
}
