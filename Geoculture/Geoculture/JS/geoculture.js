var map;
var markers = [];
var youAreHereMarker;
var error = "error";
var currentTitle = null;

function isEng() {
    var root = document.URL;
    return root.substr(root.length - 3, 3) == '/en';
}
function getRoot() {
    var root = document.URL;
    if (isEng()) {
        root = root.substr(0, root.length - 3);
    }
    return root;
}

function getLocation() {
    closeInfo();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (x) { map_initialize(x, true) }, no_geolocation, { 'timeout': 10000 });
    }
    else {
        no_geolocation();
    }
}

function no_geolocation() {
    var position = {
        'coords': {
            'latitude': 59.95,
            'longitude': 30.316667
        }
    };
    map_initialize(position, false);
}

function map_initialize(position, geolocation) {
    var mapOptions = {
        center: new google.maps.LatLng(position.coords.latitude, position.coords.longitude),
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("mapCanvas"), mapOptions);
    loadInstitutions();
    if (geolocation) {
        youAreHereMarker = new google.maps.Marker({
            position: mapOptions.center, map: map,
            icon: new google.maps.MarkerImage('http://google.com/mapfiles/arrow.png')
        });
    } else {
        youAreHereMarker = null;
    }
}

function getContent(institution) {
    var content = institution.Name + "<br>";
    var eng = isEng();
    if (institution.Email)
        content += "email: " + institution.Email + "<br>";
    if (institution.Phone)
        content += (eng ? "phone: " : "телефон: ") + institution.Phone + "<br>";
    if (institution.Site) {
        content += (eng ? 'website: ' : 'сайт: ');
        var sites = institution.Site.split(',');
        var http = 'http://';
        for (var i = 0; i < sites.length; i++) {
            var site = sites[i].trim();
            if (!(site.substr(0, http.length) == http))
                site = http + site;
            if (i > 0)
                content += ', ';
            content += '<a href="' + site + '">' + site + '</a>';
        }
        content += '<br>';
    }
    if (institution.WorkingHours)
        content += (eng ? "working hours: " : "время работы: ") + institution.WorkingHours;
    return content;
}

function insertMarkers(data) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
    institutions = JSON.parse(data);
    var infoWindow = new google.maps.InfoWindow();
    for (var i = 0; i < institutions.length ; i++) {
        var institution = institutions[i];
        var position = new google.maps.LatLng(institution.Lat, institution.Lng);
        var marker = new google.maps.Marker({ position: position, map: map, icon: new google.maps.MarkerImage(institution.Icon) });
        makeInfoWindowEvent(map, infoWindow, institution, marker);
        markers.push(marker);
    }
    if (youAreHereMarker != null) {
        makeInfoWindowEvent(map, infoWindow, null, youAreHereMarker);
    }
    google.maps.event.addListener(infoWindow, 'closeclick', function () {
        currentTitle = null;
        closeInfo();
    });
}

function makeInfoWindowEvent(map, infowindow, institution, marker) {
    google.maps.event.addListener(marker, 'click', function () {
        var content = (institution == null) ? "You are here" : getContent(institution);
        map.panTo(marker.getPosition());
        infowindow.setContent(content);
        infowindow.open(map, marker);
        if (institution != null) {
            closeInfo();
            loadWiki(institution);
        }
    });
}

function correctWikiLinks(data) {
    data = data.replace(new RegExp("/wiki/", "g"), "http://ru.wikipedia.org/wiki/")
    return data;
}

function loadWiki(institution) {
    currentTitle = institution.Name;
    $.post(getRoot() + "/Home/Wiki", { title: institution.Name }, function (data) {
        if (data != error && currentTitle == institution.Name) {
            $("#tdInfo").fadeIn("fast");
            $("#wikiData").html(correctWikiLinks(data));
        }
    });
}

function loadInstitutions() {
    inputArray = document.getElementsByTagName("input");
    $.post(getRoot() + "/Home/Institutions", {
        ch1: inputArray[0].checked, ch2: inputArray[1].checked, ch3: inputArray[2].checked,
        ch4: inputArray[3].checked, ch5: inputArray[4].checked, ch6: inputArray[5].checked,
        ch7: inputArray[6].checked, ch8: inputArray[7].checked
    },
        insertMarkers);
}

function closeInfo() {
    $("#tdInfo").fadeOut("fast");
}