var push = document.getElementById('push');
var tasks = document.querySelector('#tasks');
const requestUrl = 'https://localhost:5001';

var request = new XMLHttpRequest();

(function() {
    getTodo();
})();

function sendTodo(title) {
    request.onload = function() {
        var response = JSON.parse(request.responseText);
        console.log(response);

        if (request.status <= 200) {
            addTask(document.querySelector('#newtask input').value, response.data.id);
        } else {
            alert(response);
        }
    }

    var data = {
        'title': title,
        'description': 'js send this. we have not a data for this',
    };

    request.open('POST', `${requestUrl}/todo`);
    request.setRequestHeader('Content-Type', 'application/json');
    request.send(JSON.stringify(data));
}

function getTodo() {
    request.onload = function() {
        var response = JSON.parse(request.responseText);

        for (var i = 0; i < response.length; i++) {
            addTask(response[i].title, response[i].id);
        }
    }

    request.open('GET', `${requestUrl}/todo`);
    request.send();
}

const pushButton = function(e) {
    if (document.querySelector('#newtask input').value.length == 0) {
        alert('Please enter a task');
    } else {
        sendTodo(document.querySelector('#newtask input').value);
    }
}

function addTask(value, id) {
    tasks.innerHTML +=
        `<div class="task" data-id="${id}">
        <span>
            ${value}
        </span>
        <button class="delete">
            <i class="far fa-trash-alt"></i>
        </button>
     </div>`;

    var current_task = document.querySelector(`[data-id='${id}']`).lastElementChild;
    console.log(current_task);
    current_task.addEventListener('click', function() {
        deleteTask(id);

        if (request.status <= 200) {
            console.log(this.parentNode[id].remove());
            this.parentNode[id].remove();
        } else {
            alert('error task has error');
        }
    })

    // var current_tasks = document.querySelectorAll(".delete");
    // for (var i = 0; i < current_tasks.length; i++) {
    //     current_tasks[i].onclick = function() {
    //         deleteTask(id);

    //         if (request.status <= 200) {
    //             console.log(this.parentNode[id].remove());
    //             this.parentNode[id].remove();
    //         } else {
    //             alert('error task has error');
    //         }
    //     }
    // }
}

function deleteTask(id) {
    // request.onload = function() {

    //     // for (var i = 0; i < response.length; i++) {
    //     //     addTask(response[i].title, response[i].id);
    //     // }
    // }

    request.open('DELETE', `
        $ { requestUrl }
        /todo?id=${id}`);
    request.send();
}

push.addEventListener('click', pushButton);