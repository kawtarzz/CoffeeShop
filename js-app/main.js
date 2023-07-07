const url = "https://localhost:5001/api/beanvariety/";

const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            beanVarieties.forEach(beanVariety => {
                let beanString = JSON.stringify(beanVariety); // serialize to JSON string
                let beanObj = JSON.parse(beanString); // de-serialize to js obj
                let list = document.createElement("ul");

                for (let key in beanObj) {
                    let listItem = document.createElement("li");
                    let text = document.createTextNode(key + ": " + beanObj[key]);
                    listItem.appendChild(text);
                    list.appendChild(listItem);
                }
                document.getElementById("bean-variety-list").appendChild(list);
            });
        })
});
function getAllBeanVarieties() {
    return fetch(url).then(resp => resp.json());
}