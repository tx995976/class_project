import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelresComponent } from './panelres.component';

describe('PanelresComponent', () => {
  let component: PanelresComponent;
  let fixture: ComponentFixture<PanelresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PanelresComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
