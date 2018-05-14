class Main extends BaseController {
    private Message: string = "Hi";
    private SelectedStrategy: number = 0;


    public async Test() {
        let test = await this.Call('http://api.forex.local/api/strategy/strategytester', "Get");
        this.Model = JSON.parse(test);
        console.log(this.Model);
        
    }

    public async Change() {
        let test = await this.Call('http://api.forex.local/api/strategy/strategytester', "Get");

    }
}

window.onload = () => {
    let s = new Main();
    s.Init();
    s.Test();
}
