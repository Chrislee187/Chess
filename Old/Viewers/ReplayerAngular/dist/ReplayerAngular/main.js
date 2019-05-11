(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["main"],{

/***/ "./src/$$_lazy_route_resource lazy recursive":
/*!**********************************************************!*\
  !*** ./src/$$_lazy_route_resource lazy namespace object ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncaught exception popping up in devtools
	return Promise.resolve().then(function() {
		var e = new Error('Cannot find module "' + req + '".');
		e.code = 'MODULE_NOT_FOUND';
		throw e;
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "./src/$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "./src/app/app.component.html":
/*!************************************!*\
  !*** ./src/app/app.component.html ***!
  \************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<!--The content below is only a placeholder and can be replaced.-->\n<div class=\"root-container\">\n  <div class=\"header\">\n      <h1>{{title}}</h1>\n  </div>\n  <div class=\"content-container\">\n    <div class=\"header-container\">\n      <app-header [game]='game'></app-header>\n    </div>\n    <div class=\"board-movelist-container\">\n      <div class=\"board-container\"><app-board [game]='game' [boardKey]='boardKey'></app-board></div>\n      <div class=\"movelist-container\">\n        <app-movelist \n          [moves]='game.moves' \n          (nextMoveEvent)=\"makeMove($event)\"\n          (resetBoardEvent)=\"resetBoard()\"\n        ></app-movelist></div>\n    </div>\n    <div class=\"notes-container\"><app-notes></app-notes></div>\n  </div>\n</div> "

/***/ }),

/***/ "./src/app/app.component.scss":
/*!************************************!*\
  !*** ./src/app/app.component.scss ***!
  \************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".root-container {\n  font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;\n  min-width: 400px; }\n  .root-container .header H1 {\n    font-weight: bold; }\n  .root-container .content-container {\n    height: 700px; }\n  .root-container .content-container .nav-container {\n      height: 10%; }\n  .root-container .content-container .board-movelist-container {\n      display: flex;\n      flex-wrap: wrap;\n      flex-direction: row;\n      height: 70%; }\n  .root-container .content-container .board-movelist-container .board-container {\n        width: 60%;\n        min-width: 400px; }\n  .root-container .content-container .board-movelist-container .movelist-container {\n        width: 40%; }\n  .root-container .content-container .notes-container {\n      width: 100%;\n      height: 20%; }\n"

/***/ }),

/***/ "./src/app/app.component.ts":
/*!**********************************!*\
  !*** ./src/app/app.component.ts ***!
  \**********************************/
/*! exports provided: AppComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppComponent", function() { return AppComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _models_sample_pgn__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./models/sample.pgn */ "./src/app/models/sample.pgn.ts");
/* harmony import */ var _models_PgnJson__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./models/PgnJson */ "./src/app/models/PgnJson.ts");
/* harmony import */ var _services_chess_board_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./services/chess-board.service */ "./src/app/services/chess-board.service.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var AppComponent = /** @class */ (function () {
    function AppComponent(chessBoardService) {
        this.chessBoardService = chessBoardService;
        this.title = 'PGN Replay (Angular)';
        this.game = new _models_sample_pgn__WEBPACK_IMPORTED_MODULE_1__["WikiPgn"]();
    }
    // tslint:disable-next-line:use-life-cycle-interface
    AppComponent.prototype.ngOnInit = function () {
        this.boardKey = this.chessBoardService.generateSubscriberBoard();
        this.chessBoard = this.chessBoardService.get(this.boardKey);
    };
    AppComponent.prototype.makeMove = function (move) {
        this.chessBoard.move(move.from.toString(), move.to.toString());
    };
    AppComponent.prototype.resetBoard = function () {
        this.chessBoard.resetBoard();
    };
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", _models_PgnJson__WEBPACK_IMPORTED_MODULE_2__["PgnJson"])
    ], AppComponent.prototype, "game", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", String)
    ], AppComponent.prototype, "boardKey", void 0);
    AppComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-root',
            template: __webpack_require__(/*! ./app.component.html */ "./src/app/app.component.html"),
            styles: [__webpack_require__(/*! ./app.component.scss */ "./src/app/app.component.scss")]
        }),
        __metadata("design:paramtypes", [_services_chess_board_service__WEBPACK_IMPORTED_MODULE_3__["ChessBoardService"]])
    ], AppComponent);
    return AppComponent;
}());



/***/ }),

/***/ "./src/app/app.module.ts":
/*!*******************************!*\
  !*** ./src/app/app.module.ts ***!
  \*******************************/
/*! exports provided: AppModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppModule", function() { return AppModule; });
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/platform-browser */ "./node_modules/@angular/platform-browser/fesm5/platform-browser.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./app.component */ "./src/app/app.component.ts");
/* harmony import */ var _components_header_header_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./components/header/header.component */ "./src/app/components/header/header.component.ts");
/* harmony import */ var _components_board_board_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./components/board/board.component */ "./src/app/components/board/board.component.ts");
/* harmony import */ var _components_movelist_movelist_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./components/movelist/movelist.component */ "./src/app/components/movelist/movelist.component.ts");
/* harmony import */ var _components_notes_notes_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./components/notes/notes.component */ "./src/app/components/notes/notes.component.ts");
/* harmony import */ var _components_empty_border_square_empty_border_square_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./components/empty-border-square/empty-border-square.component */ "./src/app/components/empty-border-square/empty-border-square.component.ts");
/* harmony import */ var _components_rank_border_square_rank_border_square_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./components/rank-border-square/rank-border-square.component */ "./src/app/components/rank-border-square/rank-border-square.component.ts");
/* harmony import */ var _components_file_border_square_file_border_square_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./components/file-border-square/file-border-square.component */ "./src/app/components/file-border-square/file-border-square.component.ts");
/* harmony import */ var _components_board_square_board_square_component__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./components/board-square/board-square.component */ "./src/app/components/board-square/board-square.component.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};











var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["NgModule"])({
            declarations: [
                _app_component__WEBPACK_IMPORTED_MODULE_2__["AppComponent"],
                _components_header_header_component__WEBPACK_IMPORTED_MODULE_3__["HeaderComponent"],
                _components_board_board_component__WEBPACK_IMPORTED_MODULE_4__["BoardComponent"],
                _components_movelist_movelist_component__WEBPACK_IMPORTED_MODULE_5__["MovelistComponent"],
                _components_notes_notes_component__WEBPACK_IMPORTED_MODULE_6__["NotesComponent"],
                _components_empty_border_square_empty_border_square_component__WEBPACK_IMPORTED_MODULE_7__["EmptyBorderSquareComponent"],
                _components_rank_border_square_rank_border_square_component__WEBPACK_IMPORTED_MODULE_8__["RankBorderSquareComponent"],
                _components_file_border_square_file_border_square_component__WEBPACK_IMPORTED_MODULE_9__["FileBorderSquareComponent"],
                _components_board_square_board_square_component__WEBPACK_IMPORTED_MODULE_10__["BoardSquareComponent"]
            ],
            imports: [
                _angular_platform_browser__WEBPACK_IMPORTED_MODULE_0__["BrowserModule"]
            ],
            providers: [],
            bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_2__["AppComponent"]]
        })
    ], AppModule);
    return AppModule;
}());



/***/ }),

/***/ "./src/app/components/board-square/board-square.component.html":
/*!*********************************************************************!*\
  !*** ./src/app/components/board-square/board-square.component.html ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div title=\"{{titleContent}}\"\r\n[class.white-square]=\"isWhiteBackground\"\r\n[class.black-square]=\"!isWhiteBackground\"\r\n\r\n>{{pieceContent}}</div>"

/***/ }),

/***/ "./src/app/components/board-square/board-square.component.scss":
/*!*********************************************************************!*\
  !*** ./src/app/components/board-square/board-square.component.scss ***!
  \*********************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".black-square {\n  background-color: gray;\n  text-align: center;\n  line-height: 50px;\n  width: 50px;\n  height: 50px; }\n\n.white-square {\n  background-color: white;\n  text-align: center;\n  line-height: 50px;\n  width: 50px;\n  height: 50px; }\n"

/***/ }),

/***/ "./src/app/components/board-square/board-square.component.ts":
/*!*******************************************************************!*\
  !*** ./src/app/components/board-square/board-square.component.ts ***!
  \*******************************************************************/
/*! exports provided: BoardSquareComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BoardSquareComponent", function() { return BoardSquareComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _models_ChessBoard__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../models/ChessBoard */ "./src/app/models/ChessBoard.ts");
/* harmony import */ var _services_chess_board_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../services/chess-board.service */ "./src/app/services/chess-board.service.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var BoardSquareComponent = /** @class */ (function () {
    function BoardSquareComponent(chessBoardService) {
        this.chessBoardService = chessBoardService;
    }
    BoardSquareComponent.prototype.ngOnInit = function () {
        var board = this.chessBoardService.get(this.boardKey);
        this.updateContent(board.pieceAt(this.rank, this.file));
        this.setupSubscription(board);
    };
    BoardSquareComponent.prototype.setupSubscription = function (board) {
        var _this = this;
        this.chessPiece = board.observableAt(this.rank, this.file);
        this.chessPiece.subscribe(function (piece) { return _this.updateContent(piece); });
    };
    BoardSquareComponent.prototype.updateContent = function (piece) {
        this.pieceContent = piece;
        this.titleContent = _models_ChessBoard__WEBPACK_IMPORTED_MODULE_1__["ChessBoard"].squareTooltip(this.rank, this.file, piece);
    };
    Object.defineProperty(BoardSquareComponent.prototype, "isWhiteBackground", {
        get: function () {
            return _models_ChessBoard__WEBPACK_IMPORTED_MODULE_1__["ChessBoard"].isWhiteBackground(this.rank, this.file);
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", String)
    ], BoardSquareComponent.prototype, "boardKey", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", String)
    ], BoardSquareComponent.prototype, "rank", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", Number)
    ], BoardSquareComponent.prototype, "file", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", String)
    ], BoardSquareComponent.prototype, "pieceContent", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", String)
    ], BoardSquareComponent.prototype, "titleContent", void 0);
    BoardSquareComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            // tslint:disable-next-line:component-selector
            selector: 'board-square',
            template: __webpack_require__(/*! ./board-square.component.html */ "./src/app/components/board-square/board-square.component.html"),
            styles: [__webpack_require__(/*! ./board-square.component.scss */ "./src/app/components/board-square/board-square.component.scss")]
        }),
        __metadata("design:paramtypes", [_services_chess_board_service__WEBPACK_IMPORTED_MODULE_2__["ChessBoardService"]])
    ], BoardSquareComponent);
    return BoardSquareComponent;
}());



/***/ }),

/***/ "./src/app/components/board/board.component.html":
/*!*******************************************************!*\
  !*** ./src/app/components/board/board.component.html ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"board-root\">\n  <table>\n    <tr class=\"board-border\">\n      <td>\n        <empty-border-square></empty-border-square>\n      </td>\n      <td *ngFor=\"let rank of ['A','B','C','D','E','F','G','H']\">\n        <rank-border-square rank=\"{{rank}}\"></rank-border-square>\n      </td>\n      <td>\n        <empty-border-square></empty-border-square>\n      </td>\n    </tr>\n    \n    <tr *ngFor=\"let file of [8,7,6,5,4,3,2,1]\" >\n        <td>\n          <file-border-square file=\"{{file}}\"></file-border-square>\n        </td>\n        <td *ngFor=\"let rank of ['A','B','C','D','E','F','G','H']\">\n          <board-square [rank]=rank [file]=\"file\" boardKey={{boardKey}} ></board-square></td>\n          <td>\n              <file-border-square file=\"{{file}}\"></file-border-square>\n            </td>\n        </tr>\n    <tr class=\"board-border\">\n        <td>\n          <empty-border-square></empty-border-square>\n        </td>\n        <td *ngFor=\"let rank of ['A','B','C','D','E','F','G','H']\" >\n          <rank-border-square rank=\"{{rank}}\"></rank-border-square>\n        </td>\n        <td>\n          <empty-border-square></empty-border-square>\n        </td>\n      </tr>\n    </table>\n</div>"

/***/ }),

/***/ "./src/app/components/board/board.component.scss":
/*!*******************************************************!*\
  !*** ./src/app/components/board/board.component.scss ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "TABLE {\n  margin: 0;\n  padding: 0;\n  border-spacing: 0px;\n  font-size: x-large; }\n  TABLE TD {\n    width: 50px;\n    margin: 0;\n    padding: 0;\n    background-color: brown; }\n  TABLE TR {\n    margin: 0;\n    padding: 0;\n    border-spacing: 0px; }\n  DIV {\n  height: 50px; }\n  .board-border {\n  background-color: brown; }\n  .square {\n  text-align: center; }\n"

/***/ }),

/***/ "./src/app/components/board/board.component.ts":
/*!*****************************************************!*\
  !*** ./src/app/components/board/board.component.ts ***!
  \*****************************************************/
/*! exports provided: BoardComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BoardComponent", function() { return BoardComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var _models_PgnJson__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../models/PgnJson */ "./src/app/models/PgnJson.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var BoardComponent = /** @class */ (function () {
    function BoardComponent() {
        this.boardState = [
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null]
        ];
        for (var rankIdx = 0; rankIdx < 8; rankIdx++) {
            for (var fileIdx = 0; fileIdx < 8; fileIdx++) {
                var subject = new rxjs__WEBPACK_IMPORTED_MODULE_1__["Subject"]();
                this.boardState[fileIdx][rankIdx] = subject;
            }
        }
    }
    BoardComponent.prototype.ngOnInit = function () {
    };
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", String)
    ], BoardComponent.prototype, "boardKey", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", _models_PgnJson__WEBPACK_IMPORTED_MODULE_2__["PgnJson"])
    ], BoardComponent.prototype, "game", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", Array)
    ], BoardComponent.prototype, "boardState", void 0);
    BoardComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-board',
            template: __webpack_require__(/*! ./board.component.html */ "./src/app/components/board/board.component.html"),
            styles: [__webpack_require__(/*! ./board.component.scss */ "./src/app/components/board/board.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], BoardComponent);
    return BoardComponent;
}());



/***/ }),

/***/ "./src/app/components/empty-border-square/empty-border-square.component.html":
/*!***********************************************************************************!*\
  !*** ./src/app/components/empty-border-square/empty-border-square.component.html ***!
  \***********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<span ng-readonly=\"true\" title=\"Corner\"></span>\n\n"

/***/ }),

/***/ "./src/app/components/empty-border-square/empty-border-square.component.scss":
/*!***********************************************************************************!*\
  !*** ./src/app/components/empty-border-square/empty-border-square.component.scss ***!
  \***********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/components/empty-border-square/empty-border-square.component.ts":
/*!*********************************************************************************!*\
  !*** ./src/app/components/empty-border-square/empty-border-square.component.ts ***!
  \*********************************************************************************/
/*! exports provided: EmptyBorderSquareComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EmptyBorderSquareComponent", function() { return EmptyBorderSquareComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var EmptyBorderSquareComponent = /** @class */ (function () {
    function EmptyBorderSquareComponent() {
    }
    EmptyBorderSquareComponent.prototype.ngOnInit = function () {
    };
    EmptyBorderSquareComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'empty-border-square',
            template: __webpack_require__(/*! ./empty-border-square.component.html */ "./src/app/components/empty-border-square/empty-border-square.component.html"),
            styles: [__webpack_require__(/*! ./empty-border-square.component.scss */ "./src/app/components/empty-border-square/empty-border-square.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], EmptyBorderSquareComponent);
    return EmptyBorderSquareComponent;
}());



/***/ }),

/***/ "./src/app/components/file-border-square/file-border-square.component.html":
/*!*********************************************************************************!*\
  !*** ./src/app/components/file-border-square/file-border-square.component.html ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"square\" title=\"{{title}}\">{{file}}</div>"

/***/ }),

/***/ "./src/app/components/file-border-square/file-border-square.component.scss":
/*!*********************************************************************************!*\
  !*** ./src/app/components/file-border-square/file-border-square.component.scss ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".square {\n  margin: 0;\n  padding: 0;\n  border-width: 0;\n  font-size: smaller;\n  text-align: center; }\n"

/***/ }),

/***/ "./src/app/components/file-border-square/file-border-square.component.ts":
/*!*******************************************************************************!*\
  !*** ./src/app/components/file-border-square/file-border-square.component.ts ***!
  \*******************************************************************************/
/*! exports provided: FileBorderSquareComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FileBorderSquareComponent", function() { return FileBorderSquareComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var FileBorderSquareComponent = /** @class */ (function () {
    function FileBorderSquareComponent() {
    }
    FileBorderSquareComponent.prototype.ngOnInit = function () {
        this.title = "File " + this.file;
    };
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", String)
    ], FileBorderSquareComponent.prototype, "file", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", String)
    ], FileBorderSquareComponent.prototype, "title", void 0);
    FileBorderSquareComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'file-border-square',
            template: __webpack_require__(/*! ./file-border-square.component.html */ "./src/app/components/file-border-square/file-border-square.component.html"),
            styles: [__webpack_require__(/*! ./file-border-square.component.scss */ "./src/app/components/file-border-square/file-border-square.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], FileBorderSquareComponent);
    return FileBorderSquareComponent;
}());



/***/ }),

/***/ "./src/app/components/header/header.component.html":
/*!*********************************************************!*\
  !*** ./src/app/components/header/header.component.html ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class='container'>\r\n        <div class='event'>\r\n                {{event}} in {{site}} on {{date | date:'dd/MM/yyyy'}}\r\n        </div>\r\n        <div class='vs'>\r\n                <span class=\"value\">{{white}}</span> (White)\r\n                &nbsp;vs&nbsp;\r\n                <span class=\"value\">{{black}}</span> (Black)\r\n        </div>\r\n        <div class='result'>\r\n                <label for=\"round\" class=\"label\">Round:</label>\r\n                <span class=\"value\">{{round}}</span>\r\n                &nbsp;\r\n                <span class=\"value\">{{result}}</span>\r\n        </div>\r\n</div>"

/***/ }),

/***/ "./src/app/components/header/header.component.scss":
/*!*********************************************************!*\
  !*** ./src/app/components/header/header.component.scss ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".label {\n  font-weight: bold; }\n\n.value {\n  font-style: italic; }\n"

/***/ }),

/***/ "./src/app/components/header/header.component.ts":
/*!*******************************************************!*\
  !*** ./src/app/components/header/header.component.ts ***!
  \*******************************************************/
/*! exports provided: HeaderComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HeaderComponent", function() { return HeaderComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _models_PgnJson__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../models/PgnJson */ "./src/app/models/PgnJson.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var HeaderComponent = /** @class */ (function () {
    function HeaderComponent() {
        this.event = '?';
        this.site = '?';
        this.white = '?';
        this.black = '?';
        this.result = '?';
    }
    HeaderComponent.prototype.ngOnInit = function () {
        this.event = this.game.event;
        this.site = this.game.site;
        this.date = this.game.date;
        this.white = this.game.white;
        this.black = this.game.black;
        this.round = this.game.round;
        this.result = this.game.result;
    };
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", _models_PgnJson__WEBPACK_IMPORTED_MODULE_1__["PgnJson"])
    ], HeaderComponent.prototype, "game", void 0);
    HeaderComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-header',
            template: __webpack_require__(/*! ./header.component.html */ "./src/app/components/header/header.component.html"),
            styles: [__webpack_require__(/*! ./header.component.scss */ "./src/app/components/header/header.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], HeaderComponent);
    return HeaderComponent;
}());



/***/ }),

/***/ "./src/app/components/movelist/movelist.component.html":
/*!*************************************************************!*\
  !*** ./src/app/components/movelist/movelist.component.html ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class='container'>\n    <table #movelistTable id=\"movelistTable\">\n        <thead>\n          <th (click)='resetBoard()'>reset</th>\n          <th></th><!-- <th>prev (tbd)</th> -->\n          <th (click)='nextMove()'>next</th>\n        </thead>\n    <tbody>\n    <tr *ngFor=\"let move of moves; let idx = index; even as isEven\" [attr.data-index]=\"idx\" >\n      <td *ngIf=\"moves[idx] !== undefined && isEven\" >\n        {{move.moveNumber}}.\n      </td>\n      <td *ngIf=\"moves[idx] !== undefined && isEven\" \n          (click)='gotoMove(idx)' \n          title=\"{{moves[idx].colour}} piece moves {{moves[idx]}}\"\n          [class.current]=\"idx === currentMoveIndex-1\"\n          >\n        {{moves[idx].pgn}}\n      </td>\n      <td *ngIf=\"moves[idx+1] !== undefined  && isEven\" \n        (click)='gotoMove(idx+1)' \n        title=\"{{moves[idx+1].colour}} piece moves {{moves[idx+1]}}\"\n        [class.current]=\"idx+1 === currentMoveIndex-1\"\n        >\n        {{moves[idx+1].pgn}}\n      </td>\n    </tr>\n  </tbody>\n    <!-- <td *ngIf=\"!(idx % 2)\"> -->\n    <!-- </td> -->\n  </table>\n</div>"

/***/ }),

/***/ "./src/app/components/movelist/movelist.component.scss":
/*!*************************************************************!*\
  !*** ./src/app/components/movelist/movelist.component.scss ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".container {\n  max-height: 400px;\n  width: 200px; }\n  .container table {\n    height: 400px; }\n  .container table tbody {\n      display: block;\n      overflow: auto;\n      height: 100%; }\n  .container table thead, .container table tbody tr {\n      display: table;\n      width: 100%;\n      table-layout: fixed;\n      /* even columns width , fix width of table too*/ }\n  .container table thead {\n      width: calc( 100% - 1em); }\n  .container table td.current {\n      background-color: palegoldenrod; }\n"

/***/ }),

/***/ "./src/app/components/movelist/movelist.component.ts":
/*!***********************************************************!*\
  !*** ./src/app/components/movelist/movelist.component.ts ***!
  \***********************************************************/
/*! exports provided: MovelistComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MovelistComponent", function() { return MovelistComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var MovelistComponent = /** @class */ (function () {
    function MovelistComponent() {
        this.nextMoveEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.resetBoardEvent = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.currentMoveIndex = 0;
    }
    MovelistComponent.prototype.ngOnInit = function () {
        // this.moves = this.game.moves;
    };
    MovelistComponent.prototype.gotoMove = function (index) {
        // this.currentMoveIndex = index;
        // let move = this.moves[index];
        // // TODO: Highlight currnet move;
        // this.makeMove.emit(move);
    };
    MovelistComponent.prototype.nextMove = function () {
        var move = this.moves[this.currentMoveIndex++];
        this.scrollMoveIntoView(this.currentMoveIndex - 1);
        this.nextMoveEvent.emit(move);
    };
    MovelistComponent.prototype.resetBoard = function () {
        this.resetBoardEvent.emit(true);
        this.currentMoveIndex = 0;
        this.scrollMoveIntoView(0);
    };
    MovelistComponent.prototype.scrollMoveIntoView = function (rowIndex) {
        // TODO: Move this raw dom functionality in to a service so we can mock and check it happens.
        this.tableElement.nativeElement
            .querySelector("tr[data-index='" + rowIndex + "']")
            .scrollIntoView(false);
    };
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ViewChild"])('movelistTable'),
        __metadata("design:type", _angular_core__WEBPACK_IMPORTED_MODULE_0__["ElementRef"])
    ], MovelistComponent.prototype, "tableElement", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", Array)
    ], MovelistComponent.prototype, "moves", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"])
    ], MovelistComponent.prototype, "nextMoveEvent", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"])
    ], MovelistComponent.prototype, "resetBoardEvent", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", Object)
    ], MovelistComponent.prototype, "currentMoveIndex", void 0);
    MovelistComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-movelist',
            template: __webpack_require__(/*! ./movelist.component.html */ "./src/app/components/movelist/movelist.component.html"),
            styles: [__webpack_require__(/*! ./movelist.component.scss */ "./src/app/components/movelist/movelist.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], MovelistComponent);
    return MovelistComponent;
}());



/***/ }),

/***/ "./src/app/components/notes/notes.component.html":
/*!*******************************************************!*\
  !*** ./src/app/components/notes/notes.component.html ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<p>\n<!-- Move comments go here -->\n</p>\n"

/***/ }),

/***/ "./src/app/components/notes/notes.component.scss":
/*!*******************************************************!*\
  !*** ./src/app/components/notes/notes.component.scss ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/components/notes/notes.component.ts":
/*!*****************************************************!*\
  !*** ./src/app/components/notes/notes.component.ts ***!
  \*****************************************************/
/*! exports provided: NotesComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "NotesComponent", function() { return NotesComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var NotesComponent = /** @class */ (function () {
    function NotesComponent() {
    }
    NotesComponent.prototype.ngOnInit = function () {
    };
    NotesComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-notes',
            template: __webpack_require__(/*! ./notes.component.html */ "./src/app/components/notes/notes.component.html"),
            styles: [__webpack_require__(/*! ./notes.component.scss */ "./src/app/components/notes/notes.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], NotesComponent);
    return NotesComponent;
}());



/***/ }),

/***/ "./src/app/components/rank-border-square/rank-border-square.component.html":
/*!*********************************************************************************!*\
  !*** ./src/app/components/rank-border-square/rank-border-square.component.html ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<span title=\"Rank {{rank}}\" class=\"square\">{{rank}}</span>"

/***/ }),

/***/ "./src/app/components/rank-border-square/rank-border-square.component.scss":
/*!*********************************************************************************!*\
  !*** ./src/app/components/rank-border-square/rank-border-square.component.scss ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".square {\n  margin: 0;\n  padding: 0;\n  border-width: 0;\n  font-size: smaller;\n  text-align: center; }\n"

/***/ }),

/***/ "./src/app/components/rank-border-square/rank-border-square.component.ts":
/*!*******************************************************************************!*\
  !*** ./src/app/components/rank-border-square/rank-border-square.component.ts ***!
  \*******************************************************************************/
/*! exports provided: RankBorderSquareComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "RankBorderSquareComponent", function() { return RankBorderSquareComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var RankBorderSquareComponent = /** @class */ (function () {
    function RankBorderSquareComponent() {
    }
    RankBorderSquareComponent.prototype.ngOnInit = function () {
        this.title = "Rank " + this.rank;
    };
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"])(),
        __metadata("design:type", String)
    ], RankBorderSquareComponent.prototype, "rank", void 0);
    __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"])(),
        __metadata("design:type", String)
    ], RankBorderSquareComponent.prototype, "title", void 0);
    RankBorderSquareComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'rank-border-square',
            template: __webpack_require__(/*! ./rank-border-square.component.html */ "./src/app/components/rank-border-square/rank-border-square.component.html"),
            styles: [__webpack_require__(/*! ./rank-border-square.component.scss */ "./src/app/components/rank-border-square/rank-border-square.component.scss")]
        }),
        __metadata("design:paramtypes", [])
    ], RankBorderSquareComponent);
    return RankBorderSquareComponent;
}());



/***/ }),

/***/ "./src/app/models/ChessBoard.ts":
/*!**************************************!*\
  !*** ./src/app/models/ChessBoard.ts ***!
  \**************************************/
/*! exports provided: ChessBoard */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ChessBoard", function() { return ChessBoard; });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");

/*
    NOTES/TODO

    Unit tests

    This class is effectively coupled to the components by 'rxjs'.
     -  Abstract this away using some form of Provider pattern to supply our own
        Subject/Observable interfaces, that we can implement concrete versions of using 'rxjs'.
        Not sure if it's worth it, might make testing easier
     -  The components themselves can carry on using rxjs directly as that is correct approach for Angular
     -  Will make testing of the ChessBoard easier
*/
var ChessBoard = /** @class */ (function () {
    function ChessBoard() {
        this.moveHistory = [];
        this.currentBoardState = [
            ['r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'],
            ['p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            ['P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'],
            ['R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R']
        ];
        this.startingPieces = [
            ['r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'],
            ['p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            ['P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'],
            ['R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R']
        ];
        this.subjects = [
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null],
            [null, null, null, null, null, null, null, null]
        ];
        for (var rankIdx = 0; rankIdx < 8; rankIdx++) {
            for (var fileIdx = 0; fileIdx < 8; fileIdx++) {
                var subject = this.createBoardStateUpdateSubscription(rankIdx, fileIdx);
                this.subjects[fileIdx][rankIdx] = subject;
            }
        }
    }
    ChessBoard.squareTooltip = function (rank, file, piece) {
        var location = "" + rank + file;
        if (this.pieceNameText(piece) === '') {
            return location;
        }
        var colour = this.pieceColourText(piece);
        return colour + " " + this.pieceNameText(piece) + " at " + location;
    };
    ChessBoard.rankCharToIndex = function (rank) {
        return rank.charCodeAt(0) - 65;
    };
    ChessBoard.pieceColourText = function (piece) {
        var colour = 'Black';
        if (this.isWhitePiece(piece)) {
            colour = 'White';
        }
        return colour;
    };
    ChessBoard.isWhitePiece = function (piece) {
        var p = piece.charAt(0);
        return p.toLocaleUpperCase() === p;
    };
    ChessBoard.pieceNameText = function (piece) {
        return this.pieceNames[piece];
    };
    ChessBoard.isWhiteBackground = function (rank, file) {
        var r = this.rankCharToIndex(rank) + 1;
        var sum = r + file;
        var isWhite = (sum % 2) === 0;
        return isWhite;
    };
    ChessBoard.prototype.dumpBoard = function () {
        for (var rankIdx = 0; rankIdx < 8; rankIdx++) {
            var row = '';
            for (var fileIdx = 0; fileIdx < 8; fileIdx++) {
                var p = this.currentBoardState[rankIdx][fileIdx];
                row = row + p;
            }
        }
    };
    ChessBoard.prototype.resetBoard = function () {
        for (var rankIdx = 0; rankIdx < 8; rankIdx++) {
            var row = '';
            for (var fileIdx = 0; fileIdx < 8; fileIdx++) {
                var p = this.startingPieces[rankIdx][fileIdx];
                this.currentBoardState[rankIdx][fileIdx] = p;
                var s = this.subjects[rankIdx][fileIdx];
                s.next(p);
            }
        }
    };
    ChessBoard.prototype.createBoardStateUpdateSubscription = function (rank, file) {
        var _this = this;
        var subject = new rxjs__WEBPACK_IMPORTED_MODULE_0__["Subject"]();
        subject.subscribe(function (piece) {
            _this.currentBoardState[file][rank] = piece;
            // console.log(`square at ${rank}${file} updated to '${piece}'`);
        });
        return subject;
    };
    ChessBoard.prototype.pieceAt = function (rank, file) {
        var r = ChessBoard.rankCharToIndex(rank);
        var f = 8 - file;
        return this.currentBoardState[f][r];
    };
    ChessBoard.prototype.observableAt = function (rank, file) {
        var r = ChessBoard.rankCharToIndex(rank);
        var f = 8 - file;
        return this.subjects[f][r];
    };
    ChessBoard.prototype.subjectAt = function (rank, file) {
        return this.observableAt(rank, file);
    };
    ChessBoard.prototype.move = function (from, to) {
        var _this = this;
        var fr = from.charAt(0);
        var ff = Number(from.charAt(1));
        var tr = to.charAt(0);
        var tf = Number(to.charAt(1));
        var fromPiece = this.pieceAt(fr, ff);
        if (this.isCastlingMove(fromPiece, fr, tr)) {
            var f = function () { return _this.performCastle(fr, ff, tr); };
            this.moveHistory.push(f);
            f();
        }
        else {
            var f = function () { return _this.performMove(fr, ff, tr, tf); };
            this.moveHistory.push(f);
            f();
        }
    };
    ChessBoard.prototype.isCastlingMove = function (fromPiece, fromRankChar, toRankChar) {
        if (fromPiece === 'k' || fromPiece === 'K') {
            // check for and handle castling
            var fromRank = ChessBoard.rankCharToIndex(fromRankChar);
            var toRank = ChessBoard.rankCharToIndex(toRankChar);
            var diff = Math.abs(fromRank - toRank);
            if (diff > 1) {
                // out work castling moves
                return true;
            }
        }
        return false;
    };
    ChessBoard.prototype.performCastle = function (fromRankChar, file, toRankChar) {
        var castleFrom;
        var castleTo;
        if (toRankChar === 'G') {
            castleFrom = "H";
            castleTo = "F";
        }
        else {
            castleFrom = "A";
            castleTo = "D";
        }
        this.performMove(fromRankChar, file, toRankChar, file);
        this.performMove(castleFrom, file, castleTo, file);
    };
    ChessBoard.prototype.performMove = function (fromRankChar, fromFile, toRankChar, toFile) {
        var fromPiece = this.pieceAt(fromRankChar, fromFile);
        var fromSubject = this.subjectAt(fromRankChar, fromFile);
        var toSubject = this.subjectAt(toRankChar, toFile);
        fromSubject.next(' ');
        toSubject.next(fromPiece);
    };
    ChessBoard.prototype.testmove = function () {
        // NOTE: This method only exists for test purposes until proper moving is implemented;
        this.move('A2', 'A4');
        this.move('D7', 'D5');
        // this.dumpBoard();
    };
    ChessBoard.pieceNames = {
        'p': 'Pawn',
        'P': 'Pawn',
        'r': 'Rook',
        'R': 'Rook',
        'n': 'Knight',
        'N': 'Knight',
        'b': 'Bishop',
        'B': 'Bishop',
        'q': 'Queen',
        'Q': 'Queen',
        'k': 'King',
        'K': 'King',
        '.': '',
        ' ': '',
    };
    return ChessBoard;
}());



/***/ }),

/***/ "./src/app/models/PgnJson.ts":
/*!***********************************!*\
  !*** ./src/app/models/PgnJson.ts ***!
  \***********************************/
/*! exports provided: PgnJson */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PgnJson", function() { return PgnJson; });
var PgnJson = /** @class */ (function () {
    function PgnJson() {
        var _this = this;
        this.moves = [];
        this.toString = function () {
            return _this.white + " vs " + _this.black + " @ " + _this.event + " round " + _this.round;
        };
    }
    return PgnJson;
}());



/***/ }),

/***/ "./src/app/models/PgnJsonLocation.ts":
/*!*******************************************!*\
  !*** ./src/app/models/PgnJsonLocation.ts ***!
  \*******************************************/
/*! exports provided: PgnJsonLocation */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PgnJsonLocation", function() { return PgnJsonLocation; });
var PgnJsonLocation = /** @class */ (function () {
    function PgnJsonLocation() {
        var _this = this;
        this.toString = function () {
            return "" + _this.rank + _this.file;
        };
    }
    return PgnJsonLocation;
}());



/***/ }),

/***/ "./src/app/models/PgnJsonMove.ts":
/*!***************************************!*\
  !*** ./src/app/models/PgnJsonMove.ts ***!
  \***************************************/
/*! exports provided: PgnJsonMove */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PgnJsonMove", function() { return PgnJsonMove; });
var PgnJsonMove = /** @class */ (function () {
    function PgnJsonMove() {
        var _this = this;
        this.toString = function () {
            return _this.from + "-" + _this.to;
        };
    }
    Object.defineProperty(PgnJsonMove.prototype, "isWhitemove", {
        get: function () {
            return this.moveIndex % 2 === 0;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PgnJsonMove.prototype, "isBlackmove", {
        get: function () {
            return !this.isWhitemove;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PgnJsonMove.prototype, "moveNumber", {
        get: function () {
            return Math.floor(this.moveIndex / 2 + 1);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PgnJsonMove.prototype, "colour", {
        get: function () {
            return this.isWhitemove ? 'White' : 'Black';
        },
        enumerable: true,
        configurable: true
    });
    return PgnJsonMove;
}());



/***/ }),

/***/ "./src/app/models/pgn.ts":
/*!*******************************!*\
  !*** ./src/app/models/pgn.ts ***!
  \*******************************/
/*! exports provided: PgnJson, PgnJsonLocation, PgnJsonMove */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _PgnJson__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./PgnJson */ "./src/app/models/PgnJson.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "PgnJson", function() { return _PgnJson__WEBPACK_IMPORTED_MODULE_0__["PgnJson"]; });

/* harmony import */ var _PgnJsonLocation__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./PgnJsonLocation */ "./src/app/models/PgnJsonLocation.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "PgnJsonLocation", function() { return _PgnJsonLocation__WEBPACK_IMPORTED_MODULE_1__["PgnJsonLocation"]; });

/* harmony import */ var _PgnJsonMove__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./PgnJsonMove */ "./src/app/models/PgnJsonMove.ts");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "PgnJsonMove", function() { return _PgnJsonMove__WEBPACK_IMPORTED_MODULE_2__["PgnJsonMove"]; });






/***/ }),

/***/ "./src/app/models/sample.pgn.ts":
/*!**************************************!*\
  !*** ./src/app/models/sample.pgn.ts ***!
  \**************************************/
/*! exports provided: WikiPgn */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "WikiPgn", function() { return WikiPgn; });
/* harmony import */ var _pgn__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./pgn */ "./src/app/models/pgn.ts");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

var WikiPgn = /** @class */ (function (_super) {
    __extends(WikiPgn, _super);
    function WikiPgn() {
        var _this = _super.call(this) || this;
        _this.wikiGame = {
            "Event": "F/S Return Match",
            "Site": "Belgrade, Serbia JUG",
            "Date": "1992.11.04",
            "Round": "29",
            "White": "Fischer, Robert J.",
            "Black": "Spassky, Boris V.",
            "Result": "Draw",
            "Moves": [
                {
                    "From": "E2",
                    "To": "E4",
                    "PgnText": "e4"
                },
                {
                    "From": "E7",
                    "To": "E5",
                    "PgnText": "e5"
                },
                {
                    "From": "G1",
                    "To": "F3",
                    "PgnText": "Nf3"
                },
                {
                    "From": "B8",
                    "To": "C6",
                    "PgnText": "Nc6"
                },
                {
                    "From": "F1",
                    "To": "B5",
                    "PgnText": "Bb5"
                },
                {
                    "From": "A7",
                    "To": "A6",
                    "PgnText": "a6",
                    "Comment": "This opening is called the Ruy Lopez."
                },
                {
                    "From": "B5",
                    "To": "A4",
                    "PgnText": "Ba4"
                },
                {
                    "From": "G8",
                    "To": "F6",
                    "PgnText": "Nf6"
                },
                {
                    "From": "E1",
                    "To": "G1",
                    "PgnText": "O-O"
                },
                {
                    "From": "F8",
                    "To": "E7",
                    "PgnText": "Be7"
                },
                {
                    "From": "F1",
                    "To": "E1",
                    "PgnText": "Re1"
                },
                {
                    "From": "B7",
                    "To": "B5",
                    "PgnText": "b5"
                },
                {
                    "From": "A4",
                    "To": "B3",
                    "PgnText": "Bb3"
                },
                {
                    "From": "D7",
                    "To": "D6",
                    "PgnText": "d6"
                },
                {
                    "From": "C2",
                    "To": "C3",
                    "PgnText": "c3"
                },
                {
                    "From": "E8",
                    "To": "G8",
                    "PgnText": "O-O"
                },
                {
                    "From": "H2",
                    "To": "H3",
                    "PgnText": "h3"
                },
                {
                    "From": "C6",
                    "To": "B8",
                    "PgnText": "Nb8"
                },
                {
                    "From": "D2",
                    "To": "D4",
                    "PgnText": "d4"
                },
                {
                    "From": "B8",
                    "To": "D7",
                    "PgnText": "Nbd7"
                },
                {
                    "From": "C3",
                    "To": "C4",
                    "PgnText": "c4"
                },
                {
                    "From": "C7",
                    "To": "C6",
                    "PgnText": "c6"
                },
                {
                    "From": "C4",
                    "To": "B5",
                    "PgnText": "cxb5"
                },
                {
                    "From": "A6",
                    "To": "B5",
                    "PgnText": "axb5"
                },
                {
                    "From": "B1",
                    "To": "C3",
                    "PgnText": "Nc3"
                },
                {
                    "From": "C8",
                    "To": "B7",
                    "PgnText": "Bb7"
                },
                {
                    "From": "C1",
                    "To": "G5",
                    "PgnText": "Bg5"
                },
                {
                    "From": "B5",
                    "To": "B4",
                    "PgnText": "b4"
                },
                {
                    "From": "C3",
                    "To": "B1",
                    "PgnText": "Nb1"
                },
                {
                    "From": "H7",
                    "To": "H6",
                    "PgnText": "h6"
                },
                {
                    "From": "G5",
                    "To": "H4",
                    "PgnText": "Bh4"
                },
                {
                    "From": "C6",
                    "To": "C5",
                    "PgnText": "c5"
                },
                {
                    "From": "D4",
                    "To": "E5",
                    "PgnText": "dxe5"
                },
                {
                    "From": "F6",
                    "To": "E4",
                    "PgnText": "Nxe4"
                },
                {
                    "From": "H4",
                    "To": "E7",
                    "PgnText": "Bxe7"
                },
                {
                    "From": "D8",
                    "To": "E7",
                    "PgnText": "Qxe7"
                },
                {
                    "From": "E5",
                    "To": "D6",
                    "PgnText": "exd6"
                },
                {
                    "From": "E7",
                    "To": "F6",
                    "PgnText": "Qf6"
                },
                {
                    "From": "B1",
                    "To": "D2",
                    "PgnText": "Nbd2"
                },
                {
                    "From": "E4",
                    "To": "D6",
                    "PgnText": "Nxd6"
                },
                {
                    "From": "D2",
                    "To": "C4",
                    "PgnText": "Nc4"
                },
                {
                    "From": "D6",
                    "To": "C4",
                    "PgnText": "Nxc4"
                },
                {
                    "From": "B3",
                    "To": "C4",
                    "PgnText": "Bxc4"
                },
                {
                    "From": "D7",
                    "To": "B6",
                    "PgnText": "Nb6"
                },
                {
                    "From": "F3",
                    "To": "E5",
                    "PgnText": "Ne5"
                },
                {
                    "From": "A8",
                    "To": "E8",
                    "PgnText": "Rae8"
                },
                {
                    "From": "C4",
                    "To": "F7",
                    "PgnText": "Bxf7"
                },
                {
                    "From": "F8",
                    "To": "F7",
                    "PgnText": "Rxf7"
                },
                {
                    "From": "E5",
                    "To": "F7",
                    "PgnText": "Nxf7"
                },
                {
                    "From": "E8",
                    "To": "E1",
                    "PgnText": "Rxe1"
                },
                {
                    "From": "D1",
                    "To": "E1",
                    "PgnText": "Qxe1"
                },
                {
                    "From": "G8",
                    "To": "F7",
                    "PgnText": "Kxf7"
                },
                {
                    "From": "E1",
                    "To": "E3",
                    "PgnText": "Qe3"
                },
                {
                    "From": "F6",
                    "To": "G5",
                    "PgnText": "Qg5"
                },
                {
                    "From": "E3",
                    "To": "G5",
                    "PgnText": "Qxg5"
                },
                {
                    "From": "H6",
                    "To": "G5",
                    "PgnText": "hxg5"
                },
                {
                    "From": "B2",
                    "To": "B3",
                    "PgnText": "b3"
                },
                {
                    "From": "F7",
                    "To": "E6",
                    "PgnText": "Ke6"
                },
                {
                    "From": "A2",
                    "To": "A3",
                    "PgnText": "a3"
                },
                {
                    "From": "E6",
                    "To": "D6",
                    "PgnText": "Kd6"
                },
                {
                    "From": "A3",
                    "To": "B4",
                    "PgnText": "axb4"
                },
                {
                    "From": "C5",
                    "To": "B4",
                    "PgnText": "cxb4"
                },
                {
                    "From": "A1",
                    "To": "A5",
                    "PgnText": "Ra5"
                },
                {
                    "From": "B6",
                    "To": "D5",
                    "PgnText": "Nd5"
                },
                {
                    "From": "F2",
                    "To": "F3",
                    "PgnText": "f3"
                },
                {
                    "From": "B7",
                    "To": "C8",
                    "PgnText": "Bc8"
                },
                {
                    "From": "G1",
                    "To": "F2",
                    "PgnText": "Kf2"
                },
                {
                    "From": "C8",
                    "To": "F5",
                    "PgnText": "Bf5"
                },
                {
                    "From": "A5",
                    "To": "A7",
                    "PgnText": "Ra7"
                },
                {
                    "From": "G7",
                    "To": "G6",
                    "PgnText": "g6"
                },
                {
                    "From": "A7",
                    "To": "A6",
                    "PgnText": "Ra6"
                },
                {
                    "From": "D6",
                    "To": "C5",
                    "PgnText": "Kc5"
                },
                {
                    "From": "F2",
                    "To": "E1",
                    "PgnText": "Ke1"
                },
                {
                    "From": "D5",
                    "To": "F4",
                    "PgnText": "Nf4"
                },
                {
                    "From": "G2",
                    "To": "G3",
                    "PgnText": "g3"
                },
                {
                    "From": "F4",
                    "To": "H3",
                    "PgnText": "Nxh3"
                },
                {
                    "From": "E1",
                    "To": "D2",
                    "PgnText": "Kd2"
                },
                {
                    "From": "C5",
                    "To": "B5",
                    "PgnText": "Kb5"
                },
                {
                    "From": "A6",
                    "To": "D6",
                    "PgnText": "Rd6"
                },
                {
                    "From": "B5",
                    "To": "C5",
                    "PgnText": "Kc5"
                },
                {
                    "From": "D6",
                    "To": "A6",
                    "PgnText": "Ra6"
                },
                {
                    "From": "H3",
                    "To": "F2",
                    "PgnText": "Nf2"
                },
                {
                    "From": "G3",
                    "To": "G4",
                    "PgnText": "g4"
                },
                {
                    "From": "F5",
                    "To": "D3",
                    "PgnText": "Bd3"
                },
                {
                    "From": "A6",
                    "To": "E6",
                    "PgnText": "Re6"
                }
            ]
        };
        _this.event = _this.wikiGame.Event;
        _this.site = _this.wikiGame.Site;
        _this.date = new Date(_this.wikiGame.Date);
        _this.round = new Number(_this.wikiGame.Round);
        _this.white = _this.wikiGame.White;
        _this.black = _this.wikiGame.Black;
        _this.result = _this.wikiGame.Result;
        var moveIndex = 0;
        _this.wikiGame.Moves.forEach(function (element) {
            var jsonMove = new _pgn__WEBPACK_IMPORTED_MODULE_0__["PgnJsonMove"]();
            jsonMove.from = element.From;
            jsonMove.to = element.To;
            jsonMove.pgn = element.PgnText;
            jsonMove.comment = element.Comment;
            jsonMove.moveIndex = moveIndex++;
            _this.moves.push(jsonMove);
        });
        return _this;
    }
    return WikiPgn;
}(_pgn__WEBPACK_IMPORTED_MODULE_0__["PgnJson"]));



/***/ }),

/***/ "./src/app/services/chess-board.service.ts":
/*!*************************************************!*\
  !*** ./src/app/services/chess-board.service.ts ***!
  \*************************************************/
/*! exports provided: ChessBoardService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ChessBoardService", function() { return ChessBoardService; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _models_ChessBoard__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../models/ChessBoard */ "./src/app/models/ChessBoard.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ChessBoardService = /** @class */ (function () {
    function ChessBoardService() {
        this.matches = new Map();
        this.id = 0;
    }
    ChessBoardService.prototype.generateSubscriberBoard = function () {
        this.id++;
        this.matches.set(this.id.toString(), new _models_ChessBoard__WEBPACK_IMPORTED_MODULE_1__["ChessBoard"]());
        return this.id.toString();
    };
    ChessBoardService.prototype.get = function (boardKey) {
        if (this.matches.has(boardKey)) {
            return this.matches.get(boardKey);
        }
        throw new Error("Board with key '" + boardKey + "' not found");
    };
    ChessBoardService = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Injectable"])({
            providedIn: 'root'
        }),
        __metadata("design:paramtypes", [])
    ], ChessBoardService);
    return ChessBoardService;
}());



/***/ }),

/***/ "./src/environments/environment.ts":
/*!*****************************************!*\
  !*** ./src/environments/environment.ts ***!
  \*****************************************/
/*! exports provided: environment */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "environment", function() { return environment; });
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
var environment = {
    production: false
};
/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.


/***/ }),

/***/ "./src/main.ts":
/*!*********************!*\
  !*** ./src/main.ts ***!
  \*********************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser-dynamic */ "./node_modules/@angular/platform-browser-dynamic/fesm5/platform-browser-dynamic.js");
/* harmony import */ var _app_app_module__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./app/app.module */ "./src/app/app.module.ts");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./environments/environment */ "./src/environments/environment.ts");




if (_environments_environment__WEBPACK_IMPORTED_MODULE_3__["environment"].production) {
    Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["enableProdMode"])();
}
Object(_angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_1__["platformBrowserDynamic"])().bootstrapModule(_app_app_module__WEBPACK_IMPORTED_MODULE_2__["AppModule"])
    .catch(function (err) { return console.log(err); });


/***/ }),

/***/ 0:
/*!***************************!*\
  !*** multi ./src/main.ts ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! C:\src\chess\Viewers\ReplayerAngular\src\main.ts */"./src/main.ts");


/***/ })

},[[0,"runtime","vendor"]]]);
//# sourceMappingURL=main.js.map