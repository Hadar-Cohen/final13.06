function getSeriesSuccessCB(seriesNames) {
    for (const s of seriesNames) {
        str += "<option value=" + s + ">" + s + "</option>";
    }
    str += "</select>";
    $("#phView").html(str);
}
function getSeriesErrorCB(err) {
    alert("Error -cant get the Series names")
}

function showEpisodes(series) {
    var selectedText = series.options[series.selectedIndex].innerHTML;
    episodesList = "<tr>";
    console.log(selectedText);
    let api = "../api/Episodes?SeriesName=" + selectedText;
    ajaxCall("GET", api, "", getEpisodesSuccessCB, getEpisodesErrorCB);
}
function getEpisodesSuccessCB(episodes) {
    console.log(episodes);
    var i = 0;
    while (i < episodes.length) {
        episodesList += "<td class='card2' style='width:800px height: 700px'><center><b><p id='episodeTitle'>" + episodes[i].SeriesName + " season " + episodes[i].SeasonNum + "</p></b></center><img class= 'imgCard' src='" + episodes[i].ImageURL + "'>";
        episodesList += "<div id='episodeBlock'><br><b>" + episodes[i].EpisodeName + "</b></br > " + episodes[i].AirDate + "</br></br><div id='episodeOverView'>" + episodes[i].Overview + "</div></div></td>";

        if ((i + 1) % 4 == 0)
            episodesList += "</tr>";
        i++;
        if ((i + 1) % 4 == 1)
            episodesList += "<tr>";

    }
    episodesList += "</tr>";
    $("#episodesView").html(episodesList);

}
function getEpisodesErrorCB(err) {
    console.log(err);
}
function exitFunc() {
    localStorage.clear();
    document.location = 'insert_signup.html';
}