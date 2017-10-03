import {Directive, ElementRef, EventEmitter, Output} from 'angular2/core';
import {UtilsService} from '../../shared/services/utils.service';
import {FileParserService} from '../../shared/services/file-parser.service';

@Directive({
  providers: [UtilsService, FileParserService],
  selector: '[drag-drop]'
})
export class DragDrop {

  @Output() private fileDragged: EventEmitter<boolean> = new EventEmitter();
  @Output() private fileParsed: EventEmitter<string[]> = new EventEmitter();

  private lines = [];

  constructor(private el: ElementRef, private utilService: UtilsService, private fileParseService: FileParserService) {

    this.lines = new Array<string>();
    this.initEvents();
  }

  initEvents(): void {

    this.el.nativeElement.addEventListener('drop', (e) => {

      e.stopPropagation();
      e.preventDefault();

      let dt = e.dataTransfer;
      let files = dt.files;

      let fileReader = new FileReader();

      if (files.length > 0) {

        this.lines.length = 0;

        fileReader.readAsText(files[0]);
        fileReader.onloadend = () => {

          let fileContent = this.utilService.trimCommas(fileReader.result);
          this.lines = this.fileParseService.parseFile(fileContent);

          this.fileDragged.emit(false);
          this.fileParsed.emit(this.lines);


        };

      }

    }, false);

    this.el.nativeElement.addEventListener('dragenter', (e) => {
      e.stopPropagation();
      e.preventDefault();
      this.fileDragged.emit(true);
    }, false);

    this.el.nativeElement.addEventListener('dragover', (e) => {
      e.stopPropagation();
      e.preventDefault();
    }, false);

    this.el.nativeElement.addEventListener('dragleave', (e) => {
      e.stopPropagation();
      e.preventDefault();
      this.fileDragged.emit(false);
    }, false);

  }
}
