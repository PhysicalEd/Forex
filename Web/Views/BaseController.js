var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class BaseController {
    constructor() {
        this.Model = {};
        //public abstract get Title(): string;
        this.Meow = "ss";
        this.Woof = {};
    }
    Init() {
        var p = new Promise((success, fail) => __awaiter(this, void 0, void 0, function* () {
            // Bind
            try {
                yield this.BindDataToTemplate();
                success();
            }
            catch (ex) {
                console.error("Could not bind template", ex);
                fail();
            }
        }));
        return p;
    }
    //public Test() {
    //    alert("Test");
    //}
    /**
     * Sets up our two-way data binding between the model and the template
     */
    BindDataToTemplate() {
        var p = new Promise((success, fail) => {
            // Vue requires an array of functions/methods to pass to its 'methods' property, so we create an empty list here and populate below using reflection
            let functions = {};
            /*
            fn_bindFunctions
            Helper routine to iterate up the inheritance model, binding properties/functions as required. For example, if you are binding Car, which inherits from Vehicle, this will
            make sure that Vue has access to the properties in Vehicle
            */
            let fn_bindFunctions = (obj) => {
                // Get all the properties and functions etc of this object
                let fnList = Object.getOwnPropertyNames(obj);
                // Iterate through each property/function we have found
                fnList.map((propertyName) => {
                    if (typeof (propertyName) !== "string")
                        return;
                    if (propertyName === "constructor")
                        return;
                    // We prefix private functions with _ sometimes
                    if (propertyName.indexOf('_') === 0)
                        return;
                    // Map the function
                    if (this[propertyName] instanceof Function) {
                        // Break if we've already got this function name (ie. an inherited class has overriden it)
                        if (typeof (functions[propertyName]) !== "undefined")
                            return;
                        // Map to our VUE object
                        functions[propertyName] = (...args) => {
                            // By using a closure and referring to 'this' we change the javascript context of the event handler from 'vue', back to this actual controller!
                            return this[propertyName](...args);
                        };
                    }
                });
                // Bind parent object (ie. the class that this object inherits from) - note that BaseController is the name of our base class, from which all objects inherit
                if (typeof (obj.constructor) !== "undefined" &&
                    typeof (obj.constructor.name) === "string" &&
                    obj.constructor.name !== "BaseController") {
                    fn_bindFunctions(Object.getPrototypeOf(obj));
                }
            };
            // Kick off function binding with the current instance
            fn_bindFunctions(Object.getPrototypeOf(this));
            // Create our vue bindings
            var app = new Vue({
                el: "#app",
                data: this,
                methods: functions // Bind the methods to the array of functions we created above
            });
            Vue.nextTick(() => {
                //// We need to reload the view after we've bound it to VUE because otherwise our DOM elements (at least, using jquery like append()) do not take effect. Can't work out why but suspect VUE converts it to some kind of shadow copy
                //this.View = this.Container.find(`#${this.UniqueID}`);
                //this.Init();
                //this.OnRestore();
                success();
            });
        });
        return p;
    }
    Call(url, method) {
        return new Promise((resolve, reject) => {
            const request = new XMLHttpRequest();
            request.onload = function () {
                if (this.status === 200) {
                    resolve(this.response);
                }
                else {
                    reject(new Error(this.statusText));
                }
            };
            request.onerror = function () {
                reject(new Error('XMLHttpRequest Error: ' + this.statusText));
            };
            request.open(method, url);
            request.send();
        });
    }
}
window.onload = () => {
    //let s = new BaseController();
    //s.Init();
};
//# sourceMappingURL=BaseController.js.map