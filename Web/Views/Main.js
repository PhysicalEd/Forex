var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Main extends BaseController {
    constructor() {
        super(...arguments);
        this.Message = "Hi";
        this.SelectedStrategy = 0;
    }
    Test() {
        return __awaiter(this, void 0, void 0, function* () {
            let test = yield this.Call('http://api.forex.local/api/strategy/strategytester', "Get");
            this.Model = JSON.parse(test);
            console.log(this.Model);
        });
    }
    Change() {
        return __awaiter(this, void 0, void 0, function* () {
            let test = yield this.Call('http://api.forex.local/api/strategy/strategytester', "Get");
        });
    }
}
window.onload = () => {
    let s = new Main();
    s.Init();
    s.Test();
};
//# sourceMappingURL=Main.js.map