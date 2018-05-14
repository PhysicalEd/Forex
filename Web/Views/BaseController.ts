declare var Vue: any;

abstract class BaseController {
    
    protected Model: any = {};
    //public abstract get Title(): string;
    public Meow: string = "ss";
    public Woof: any = {};

    public Init(): Promise<void> {
        var p = new Promise<void>(async (success, fail) => {

            // Bind
            try {
                await this.BindDataToTemplate();
                success();
            } catch (ex) {
                console.error("Could not bind template", ex);
                fail();
            }
        });

        return p;
    }

    //public Test() {
    //    alert("Test");
    //}

    /**
     * Sets up our two-way data binding between the model and the template
     */
    private BindDataToTemplate(): Promise<void> {

        var p = new Promise<void>((success, fail) => {
            // Vue requires an array of functions/methods to pass to its 'methods' property, so we create an empty list here and populate below using reflection
            let functions: any = {};

            /*
            fn_bindFunctions
            Helper routine to iterate up the inheritance model, binding properties/functions as required. For example, if you are binding Car, which inherits from Vehicle, this will
            make sure that Vue has access to the properties in Vehicle
            */
            let fn_bindFunctions = (obj: any) => {
                // Get all the properties and functions etc of this object
                let fnList = Object.getOwnPropertyNames(obj);

                // Iterate through each property/function we have found
                fnList.map((propertyName: string) => {
                    if (typeof (propertyName) !== "string") return;
                    if (propertyName === "constructor") return;

                    // We prefix private functions with _ sometimes
                    if (propertyName.indexOf('_') === 0) return;

                    // Map the function
                    if ((this as any)[propertyName] instanceof Function) {

                        // Break if we've already got this function name (ie. an inherited class has overriden it)
                        if (typeof (functions[propertyName]) !== "undefined") return;

                        // Map to our VUE object
                        functions[propertyName] = (...args: any[]) => {
                            // By using a closure and referring to 'this' we change the javascript context of the event handler from 'vue', back to this actual controller!
                            return (this as any)[propertyName](...args);
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
                data: this,// Binding the data to 'this' works well
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

    public Call(url: string, method: string): Promise<any> {
        return new Promise<any>(
            (resolve, reject) => {
                const request = new XMLHttpRequest();
                request.onload = function() {
                    if (this.status === 200) {
                        resolve(this.response);
                    } else {
                        reject(new Error(this.statusText));
                    }
                };
                request.onerror = function() {
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
}
