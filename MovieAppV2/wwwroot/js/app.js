const questionNumber = document.querySelector(".question-number");
const questionText = document.querySelector(".question-text");
const optionContainer = document.querySelector(".option-container");

const answersIndicatorContainer = document.querySelector(".answers-indicator");

const homeBox=document.querySelector(".home-box");
const quizBox=document.querySelector(".quiz-box");
const resultBox=document.querySelector(".result-box");

let questionCounter = 0;
let currentQuestion;
let availableQuestions = [];
let avaiableOptions = [];
let correctAnswers = 0;
let attempt = 0;

function setAvailableQuestions(){
    const totalQuestion = quiz.length;
    for (let i = 0; i < totalQuestion; i++) {
        availableQuestions.push(quiz[i])
    }
}

var audio = document.getElementById("audio");
function alarma(){
    audio.play();
}




function getNewQuestion(){
    questionNumber.innerHTML = "Pregunta # "+ (questionCounter + 1) + " de "+ quiz.length;

    const questionIndex = availableQuestions[Math.floor(Math.random() * availableQuestions.length)]
    currentQuestion = questionIndex;
    questionText.innerHTML = currentQuestion.q;

    const index1 = availableQuestions.indexOf(questionIndex);
    
    availableQuestions.splice(index1,1);

    
    //console.log(questionIndex)
    //console.log(availableQuestions)
    
    //const optionLen = currentQuestion.options.length
    //console.log(currentQuestion.options)



    //revisar
    const optionLen = currentQuestion.options.length
    //push options into avaiableOptions Array    
    for (let i = 0; i < optionLen; i++) {
       avaiableOptions.push(i)
    }
    optionContainer.innerHTML='';
    let animationDelay= 0.15;
    // create options in html
    for(let i =0; i< optionLen; i++){

        const optonIndex = avaiableOptions[Math.floor(Math.random()* avaiableOptions.length)];
        
        const index2 = avaiableOptions.indexOf(optonIndex)

        avaiableOptions.splice(index2,1);     
        const option = document.createElement("div");
        option.innerHTML = currentQuestion.options[optonIndex];
        option.id = optonIndex;
        option.style.animationDelay= animationDelay + 's';
        animationDelay=animationDelay+0.15;
        option.className= "option";
        optionContainer.appendChild(option) 
        option.setAttribute("onclick", "getResult(this)");
    }
    
    //console.log(avaiableOptions)
    questionCounter++

}

function getResult(element){
    const id = parseInt(element.id);

    if(id === currentQuestion.answer){
        element.classList.add("correct");
        
        //No es necesario
        updateAnswerIndicator("correct");
        correctAnswers++;
    }else{
        
        element.classList.add("wrong");

        //No es necesario
        updateAnswerIndicator("wrong");

        
            //Para mostrar en verde cual es la respuesta correcta
        
        const optionLen = optionContainer.children.length;
        for(let i = 0; i <optionLen; i++){
            if(parseInt(optionContainer.children[i].id)=== currentQuestion.answer){
                optionContainer.children[i].classList.add("correct");
            }
        }
        
        
        
    }
    attempt++;
    unclickableOptions();
}


function unclickableOptions(){
    const optionLen = optionContainer.children.length;
    for(let i =0; i<optionLen; i++){
        optionContainer.children[i].classList.add("already-answered");
    }
}


function answersIndicator(){
    answersIndicatorContainer.innerHTML='';
    const totalQuestion = quiz.length;
    for(let i=0; i < totalQuestion; i++ ){
        const indicator = document.createElement("div");
        answersIndicatorContainer.appendChild(indicator);
    }
}

function updateAnswerIndicator(markType){
    answersIndicatorContainer.children[questionCounter-1].classList.add(markType)
}


function next(){
    if (questionCounter === quiz.length) {
        quizOver();
    }
    else{
        getNewQuestion();
    }
}

function quizOver(){
    quizBox.classList.add("hide");

    resultBox.classList.remove("hide");

    quizResult();
}


function quizResult(){
    resultBox.querySelector(".total-question").innerHTML= quiz.length;
    resultBox.querySelector(".total-attempt").innerHTML= attempt;
    resultBox.querySelector(".total-correct").innerHTML= correctAnswers;
    resultBox.querySelector(".total-wrong").innerHTML = attempt - correctAnswers;

    const percentage = (correctAnswers/quiz.length)*100;
    resultBox.querySelector(".percentage").innerHTML= percentage.toFixed(2) + "%"
    resultBox.querySelector(".total-score").innerHTML= correctAnswers + " / " + quiz.length
}

function tryAgainQuiz(){
    resultBox.classList.add("hide");

    quizBox.classList.remove("hide");
    resetQuiz();
    startQuiz()
}

function resetQuiz(){
    questionCounter = 0;
    correctAnswers = 0;
    attempt = 0;
}

function goToHome(){
    resultBox.classList.add("hide");
    homeBox.classList.remove("hide");
    resetQuiz();
}


function startQuiz(){

    homeBox.classList.add("hide");

    quizBox.classList.remove("hide")
    
    setAvailableQuestions();
    
    getNewQuestion();

    answersIndicator();

}

window.onload = function(){
    homeBox.querySelector(".total-question").innerHTML=quiz.length;
}