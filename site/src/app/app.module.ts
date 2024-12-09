import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './layouts/navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './auth/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { LoginComponent } from './auth/login/login.component';
import { LatestComponent } from './songs/latest/latest.component';
import { DetailComponent } from './songs/detail/detail.component';
import { ForYouComponent } from './songs/for-you/for-you.component';
import { MostPlayedComponent } from './songs/most-played/most-played.component';
import { SongListComponent } from './songs/song-list/song-list.component';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    RegisterComponent,
    LoginComponent,
    LatestComponent,
    DetailComponent,
    ForYouComponent,
    MostPlayedComponent,
    SongListComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [
    provideHttpClient(withFetch())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
