
(function ($) {

	"use strict";

	var fullHeight = function () {

		$('.js-fullheight').css('height', $(window).height());
		$(window).resize(function () {
			$('.js-fullheight').css('height', $(window).height());
		});

	};
	fullHeight();

	$('#sidebarCollapse').on('click', function () {
		$('#sidebar').toggleClass('active');
	});

})(jQuery);


$('#btnShow').on('click', function () {
	$('#navbarSupportedContent').toggleClass('show');
	//if ($("#navbarSupportedContent").hasClass('show')) {

	//	$('#navbarSupportedContent').css("height", 100);
	//}
	//else {

	//	$('#navbarSupportedContent').css("height", 0);
 //   }
});


const rangeInput = document.querySelectorAll(".range-input input"),
priceInput = document.querySelectorAll(".price-input input"),
range = document.querySelector(".slider .progress");
let priceGap = 1000;
priceInput.forEach(input =>{
    input.addEventListener("input", e =>{
        let minPrice = parseInt(priceInput[0].value),
        maxPrice = parseInt(priceInput[1].value);
        
        if((maxPrice - minPrice >= priceGap) && maxPrice <= rangeInput[1].max){
            if(e.target.className === "input-min"){
                rangeInput[0].value = minPrice;
                range.style.left = ((minPrice / rangeInput[0].max) * 100) + "%";
            }else{
                rangeInput[1].value = maxPrice;
                range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
            }
        }
    });
});
rangeInput.forEach(input =>{
    input.addEventListener("input", e =>{
        let minVal = parseInt(rangeInput[0].value),
        maxVal = parseInt(rangeInput[1].value);
        if((maxVal - minVal) < priceGap){
            if(e.target.className === "range-min"){
                rangeInput[0].value = maxVal - priceGap
            }else{
                rangeInput[1].value = minVal + priceGap;
            }
        }else{
            priceInput[0].value = minVal;
            priceInput[1].value = maxVal;
            range.style.left = ((minVal / rangeInput[0].max) * 100) + "%";
            range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
        }
    });
});



var btn = document.getElementById("theme-button");
var btnI = document.getElementById("theme-button").children[0];
var link = document.getElementById("theme-link");

btn.addEventListener("click", function () { ChangeTheme(); });


let lightTheme = "#";
let darkTheme = "/user/css/darkTheme.css";

function SetFirstTheme() {

    var theme = getCookie('theme');
    if (theme == 'dark') {
        btnI.classList.add('fa-moon');
        btnI.classList.remove('fa-sun');
        link.setAttribute("href", darkTheme);

    }
    else {
        btnI.classList.remove('fa-moon');
        btnI.classList.add('fa-sun');
        link.setAttribute("href", lightTheme);
    }
}

function ChangeTheme() {

    var currTheme = link.getAttribute("href");
    console.log(currTheme + '   =   ' + lightTheme);
    var theme = '';

    if (currTheme == lightTheme) {
        currTheme = darkTheme;
        theme = "dark";
    }
    else {
        currTheme = lightTheme;
        theme = "light";
    }

    btnI.classList.toggle('fa-moon');
    btnI.classList.toggle('fa-sun');
    link.setAttribute("href", currTheme);

    setCookie('theme', theme, { secure: true, 'max-age': 14400 });

}


function setCookie(name, value, options = {}) {

    options = {
        path: '/',
    };

    if (options.expires instanceof Date) {
        options.expires = options.expires.toUTCString();
    }

    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    for (let optionKey in options) {
        updatedCookie += "; " + optionKey;
        let optionValue = options[optionKey];
        if (optionValue !== true) {
            updatedCookie += "=" + optionValue;
        }
    }

    document.cookie = updatedCookie;
}

function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}



$('.row').on('click', '[data-open]', async function () {
    $('.modal-body').hide();
    $('.mySpinner').show();
    let myurl = 'https://' + window.location.host+'/' + $(this).attr('href');
    let response = await fetch(myurl)
    let result = await response.text();
    $('.modal-body').html(result);
    $('.mySpinner').hide();
    $('.modal-body').show();
}//
)
