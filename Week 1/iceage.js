
let users = [];

document.addEventListener("DOMContentLoaded", function() {
    const video = document.getElementById('background-video');
    const playPauseButton = document.getElementById('playPauseButton');
    const wheelButton = document.querySelector('.wheel');
    const loginModal = document.getElementById('loginModal');
    const closeModalButton = document.getElementById('closeModal');
    const closeBtn = document.querySelector('.close-btn');
    const image = document.querySelector('.top-left-image');

    let animationInterval; 
    let timeouts = []; 
    let animationsRunning = false; 

    video.pause();
    
    playPauseButton.addEventListener('click', function() {
        if (video.paused) {
            video.play(); 
            loginModal.style.display = "flex"; 
            video.loop = true; 
            startAnimations();  
            startSnowfall();    
        } else {
            closeModal(); 
        }
    });

    wheelButton.addEventListener('click', function() {
        wheelButton.classList.add('fall'); 
        
        setTimeout(() => {
            loginModal.style.display = "flex"; 
            startAnimations();  
            startSnowfall();    
        }, 1000);
    });

    function startAnimations() {
        if (animationsRunning) return;  
        animationsRunning = true;

        resetImageAnimations();

        let shakeTimeout = setTimeout(() => {
            image.classList.add('shake');
            let stopShake = setTimeout(() => {
                image.classList.remove('shake');
            }, 2000);
            timeouts.push(stopShake);
        }, 10000);
        timeouts.push(shakeTimeout);

        let rotateTimeout = setTimeout(() => {
            image.classList.add('rotate-and-crack');
            let stopRotate = setTimeout(() => {
                image.classList.remove('rotate-and-crack');
            }, 6820);
            timeouts.push(stopRotate);
        }, 24850);
        timeouts.push(rotateTimeout);

        animationInterval = setInterval(() => {
            resetImageAnimations();

            let shakeTimeout = setTimeout(() => {
                image.classList.add('shake');
                let stopShake = setTimeout(() => {
                    image.classList.remove('shake');
                }, 2000);
                timeouts.push(stopShake);
            }, 9650);
            timeouts.push(shakeTimeout);

            let rotateTimeout = setTimeout(() => {
                image.classList.add('rotate-and-crack');
                let stopRotate = setTimeout(() => {
                    image.classList.remove('rotate-and-crack');
                }, 6670);
                timeouts.push(stopRotate);
            }, 24500);
            timeouts.push(rotateTimeout);
        }, 31950);
    }

    function resetImageAnimations() {
        image.classList.remove('shake', 'rotate-and-crack');
        void image.offsetWidth; 
    }

    function closeModal() {
        loginModal.style.display = "none";
        video.pause();
        video.currentTime = 0;
        playPauseButton.innerText = "Tekerlek Buton"; 
        wheelButton.classList.remove('fall');

        resetImageAnimations();

        clearInterval(animationInterval);

        timeouts.forEach(clearTimeout);
        timeouts = [];

        animationsRunning = false;
    }

    closeModalButton.addEventListener('click', closeModal);
    closeBtn.addEventListener('click', closeModal);

    window.addEventListener('click', function(event) {
        if (event.target === loginModal) {
            closeModal();
        }
    });

    video.addEventListener('timeupdate', function() {
        if (video.currentTime === 0) {
            resetImageAnimations();
        }
    });

    function startSnowfall() {
        for (let i = 0; i < 35; i++) {
            let snowflake = document.createElement('div');
            snowflake.classList.add('snowflake');
            document.body.appendChild(snowflake);
            let leftPosition = Math.random() * 100;
            snowflake.style.left = leftPosition + '%';
            let animationDuration = 5 + Math.random() * 5; 
            snowflake.style.animationDuration = animationDuration + 's';
        }
    }


    document.getElementById("login-btn").addEventListener("click", function (event) {
        event.preventDefault(); 

        let username = document.getElementById("username").value;
        let password = document.getElementById("password").value;
        users.push({ username, password });
    });
});
