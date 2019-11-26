import { Component, OnInit, ViewChild } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { Location } from 'src/app/_models/Location';
import { PhotosService } from 'src/app/_services/photos.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { PlaygroundsService } from 'src/app/_services/playgrounds.service';
import { Playground } from 'src/app/_models/Playground';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  memberPhoto: Photo;
  errorWhileUpdatingPhoto: string;
  updatePhotoForm: FormGroup;
  locations: any[];
  playgrounds: any[];
  locationsKeyword = 'name';
  playgroundsKeyword = 'name';
  locationPlaceholder = 'Location';
  playgroundPlaceholder = 'Playground';
  playgroundsNotYetLoaded = true;
  selectedPlaygroundId = -1;
  locationsInitialValue = {};
  playgroundsInitialValue = {};

  @ViewChild('ngPlaygroundsAutoComplete', null) ngPlaygroundsAutoComplete;

  constructor(
    private photoService: PhotosService,
    private authService: AuthService,
    private playgroundsService: PlaygroundsService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.memberPhoto = data['photo'];
    });

    this.updatePhotoForm = new FormGroup({
      description: new FormControl(this.memberPhoto.description)
      // playground: new FormControl({id: this.memberPhoto.playgroundId, name: this.memberPhoto.playgroundAddress})
    });

    this.setInitialValues();
    this.setLocationsList();
  }

  private setInitialValues() {
    if (this.memberPhoto.playgroundLocationId > 0
      && this.memberPhoto.playgroundId > 0) {
        this.locationsInitialValue = {
          id: this.memberPhoto.playgroundLocationId,
          name: this.memberPhoto.playgroundLocationStr
        };

      this.setPlaygroundsByLocation(this.locationsInitialValue);
      this.playgroundsInitialValue = {
        id: this.memberPhoto.playgroundId,
        name: this.memberPhoto.playgroundAddress
      };
    }
  }

  private setLocationsList() {
    this.playgroundsService.getAllLoacations().subscribe(
      (locations: Location[]) => {
        if (locations) {
          this.locations = new Array();
          for (let i = 0; i < locations.length; i++) {
            this.locations.push({
              id: locations[i].id,
              name: locations[i].city + ', ' + locations[i].country
            });
          }
        }
      });
  }

  setPlaygroundsByLocation(selectedLocation) {
    if (selectedLocation !== null
        && selectedLocation !== undefined
        && selectedLocation.id > 0) {
      this.playgroundsService.getPlaygroundsByLocationId(selectedLocation.id).subscribe((playgrounds: Playground[]) => {
        if (playgrounds) {
          this.playgrounds = new Array();
          for (let i = 0; i < playgrounds.length; i++) {
            this.playgrounds.push({
              id: playgrounds[i].id,
              name: playgrounds[i].address
            });
          }
          this.playgroundsNotYetLoaded = false;
        }
      });
    }
  }

  setPlaygroundId(selectedPlayground) {
    this.selectedPlaygroundId = selectedPlayground.id;
  }

  onLocationCleared() {
    this.ngPlaygroundsAutoComplete.clear();
    this.playgrounds = [];
    this.playgroundsNotYetLoaded = true;
  }

  updatePhoto() {
    const photoModel = Object.assign({}, this.updatePhotoForm.value);
    const photoToUpdate = {
      id: this.memberPhoto.id,
      description: photoModel.description,
      publicId: this.memberPhoto.publicId,
      url: this.memberPhoto.url,
      memberId: this.memberPhoto.memberId,
      created: this.memberPhoto.created,
      playgroundId: null
    };

    if (this.selectedPlaygroundId > 0) {
      photoToUpdate.playgroundId = this.selectedPlaygroundId;
    } else {
      if (this.memberPhoto.playgroundId > 0) {
        photoToUpdate.playgroundId = this.memberPhoto.playgroundId;
      }
    }
    console.log(photoToUpdate);
    this.photoService.updatePhoto(photoToUpdate, this.authService.getMemberToken()).subscribe((photoUpdated: Photo) => {
      this.memberPhoto = photoUpdated;
    },
    error => {
      this.errorWhileUpdatingPhoto = error;
    });
  }
}
