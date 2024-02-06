const chatBody = document.querySelector(".chat-body");
const txtInput = document.querySelector("#txtInput");
const send = document.querySelector(".send");
const loadingEle = document.querySelector(".loading");
const student = document.querySelector("#bodybox1");
let st = "";
send.addEventListener("click", () => renderUserMessage());

txtInput.addEventListener("keyup", (event) => {
    if (event.keyCode === 13) {

        //myStopFunction();
        renderUserMessage();
        //const myTimeout = setTimeout(myGreeting, 3000);                
    }
});
// function myGreeting() {
//   alert("Happy Birthday!");
// }
// function myStopFunction() {
//   clearTimeout(myTimeout);
// }
const renderUserMessage = () => {

    const userInput = txtInput.value;
    renderMessageEle(userInput, "user");
    txtInput.value = "";
    toggleloading(false);
    setTimeout(() => {

        renderChatbotResponse(userInput);
        setScrollPosition();
        toggleloading(true);

    }, 2200);
};

const renderChatbotResponse = (userInput) => {
    const res = getChatbotResponse(userInput);
    const bot = "Lovely : ";
    renderMessageEle("" + bot + "" + res);

    // setTimeout(()=>
    // {
    //   renderMessageEle(""+bot+" It was nice chatting with you. Just text me. If you need something, please let me know.");
    // },30100);

};

const renderMessageEle = (txt, type) => {
    let className = "user-message";
    if (type !== "user") {
        className = "chatbot-message";
    }
    const messageEle = document.createElement("div");
    const txtNode = document.createTextNode(txt);
    messageEle.classList.add(className);
    messageEle.append(txtNode);
    chatBody.append(messageEle);
};


const getChatbotResponse = (userInput) => {

    if (st == "Student") {
        return getresponse1(userInput);
    }
    else if (st == "Interns") {
        return getresponse1(userInput);
    }
    else if (st == "Jobseekers") {
        return getresponse3(userInput);
    }
    else if (st == "Employers") {
        return getresponse4(userInput);
    }
    else if (st == "Recruitment Agency") {
        return getresponse5(userInput);
    }
    else if (st == "Institutes") {
        return getresponse6(userInput);
    }
    else if (st == "Freelancers") {
        return getresponse7(userInput);
    }
    else if (st == "Partners") {
        return getresponse8(userInput);
    }
    else {
        return getresponse(userInput);
    }
    // return responseObj[userInput] == undefined
    //   ? "Please try something else"
    //   : responseObj[userInput];
};

const setScrollPosition = () => {
    if (chatBody.scrollHeight > 0) {
        chatBody.scrollTop = chatBody.scrollHeight;
    }
};
const toggleloading = (show) => loadingEle.classList.toggle("hide", show)

function openForm() {
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}
$(function () {
    $('#bodybox1').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox1');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();
    });
    $('#bodybox2').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox2');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();

    });
    $('#bodybox3').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox3');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();
    });
    $('#bodybox4').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox4');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();
    });
    $('#bodybox5').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox5');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();
    });
    $('#bodybox6').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox6');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();
    });
    $('#bodybox7').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox7');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();
    });
    $('#bodybox8').on('click', function () {
        var text = $('#txtInput');
        var txt = $('#bodybox8');
        text.val(txt.val());
        st = txt.val();
        $(".send").click();

    });
});
