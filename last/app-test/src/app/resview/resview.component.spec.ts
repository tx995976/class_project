import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResviewComponent } from './resview.component';

describe('ResviewComponent', () => {
  let component: ResviewComponent;
  let fixture: ComponentFixture<ResviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ResviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
