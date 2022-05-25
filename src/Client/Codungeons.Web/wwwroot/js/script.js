// lobby
const lobby = []
let team1
let team2
lobby.push({ team_id: 'team1', player_id: 'player1', name: 'Kukuku' })
lobby.push({ team_id: 'team1', player_id: 'player2', name: 'ar4ol' })
lobby.push({ team_id: 'team2', player_id: 'player3', name: 'Kogosh' })
lobby.push({ team_id: 'team2', player_id: 'player4', name: 'taibibika' })

// codeblocks
const codeblocks = []
const codemirror_objects = []

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7133/editor")
    .build();


document.addEventListener('keyup', (e) => {
    const block_element = e.target.parentElement.parentElement.getElementsByClassName('CodeMirror-line')[0]
    const block_element_id = block_element.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.getElementsByTagName('textarea')[0].id

    const obj = parse_codeblock(block_element_id)
    render_changes({ element_id: block_element_id, team_id: obj.team_id, player_id: obj.player_id, block_id: obj.block_id, code: block_element.innerText })
})

hubConnection.on("Send", function (data) {
    console.log(data)
});

hubConnection.start();

initialize();

function merge_codeblock_id(team_id, player_id, block_id) {
    return `${team_id}.${player_id}.${block_id}`
}

function parse_codeblock(codeblock_id) {
    const splited_id = codeblock_id.split('.')
    return { team_id: splited_id[0], player_id: splited_id[1], block_id: splited_id[2] }
}

function initialize() {

    // codeblocks data
    for (let i = 0; i < lobby.length; i++) {

        const team_id = lobby[i].team_id
        const player_id = lobby[i].player_id
        const block_id = codeblocks.length + 1
        const element_id = merge_codeblock_id(team_id, player_id, block_id)

        codeblocks.push({ element_id: element_id, team_id: team_id, player_id: player_id, block_id: block_id, code: '' })
    }

    // display code blocks
    const editor_elements = document.getElementsByClassName('code-editor')
    team1 = lobby[0].team_id
    editor_elements[0].id = team1

    for (let i = 0; i < lobby.length; i++) {
        if (editor_elements[0].id != lobby[i].team_id) {
            team2 = lobby[i].team_id
            editor_elements[1].id = team2
            break;
        }
    }


    for (let i = 0; i < codeblocks.length; i++) {
        const codeblock_div = document.createElement('div')
        const codeblock_textarea = document.createElement('textarea')

        codeblock_div.className = 'codeblock'
        codeblock_textarea.id = codeblocks[i].element_id

        codeblock_div.appendChild(codeblock_textarea)


        document.getElementById(codeblocks[i].team_id).appendChild(codeblock_div)
    }

    for (let i = 0; i < codeblocks.length; i++) {
        codemirror_objects.push(CodeMirror.fromTextArea(document.getElementById(codeblocks[i].element_id), {
            matchBrackets: true,
            continueComments: "Enter",
            theme: "dracula"
        }))
    }
}


function render_changes(obj) {
    console.log(JSON.stringify({ team_id: obj.team_id, player_id: obj.player_id, block_id: obj.block_id, code: obj.code}))
    hubConnection.invoke("Send", obj.code);

    // change codeblocks data
    for (let i = 0; i < codeblocks.length; i++) {
        if (codeblocks[i].element_id == obj.element_id) {
            codeblocks[i].code = obj.code
            break
        }
    }

    let flag_team1 = true
    let flag_team2 = true

    // add new rows
    for (let i = 0; i < codeblocks.length; i++) {

        if (codeblocks[i].team_id == team1 && codeblocks[i].code.length == 0) {
            flag_team1 = false
            break
        }
        else if (codeblocks[i].team_id == team1 && codeblocks[i].code == '\u200b') {
            flag_team1 = false
            break
        }
    }

    for (let i = 0; i < codeblocks.length; i++) {

        if (codeblocks[i].team_id == team2 && codeblocks[i].code.length == 0) {
            flag_team2 = false
            break
        }
        else if (codeblocks[i].team_id == team2 && codeblocks[i].code == '\u200b') {
            flag_team2 = false
            break
        }
    }

    if (flag_team1) {
        for (let i = 0; i < lobby.length; i++) {
            if (lobby[i].team_id == team1) {
                const team_id = lobby[i].team_id
                const player_id = lobby[i].player_id
                const block_id = codeblocks.length + 1
                const element_id = merge_codeblock_id(team_id, player_id, block_id)
                codeblocks.push({ element_id: element_id, team_id: team_id, player_id: player_id, block_id: block_id, code: '' })

                const codeblock_div = document.createElement('div')
                const codeblock_textarea = document.createElement('textarea')

                codeblock_div.className = 'codeblock'
                codeblock_textarea.id = element_id

                codeblock_div.appendChild(codeblock_textarea)

                document.getElementById(team1).appendChild(codeblock_div)

                codemirror_objects.push(CodeMirror.fromTextArea(document.getElementById(element_id), {
                    matchBrackets: true,
                    continueComments: "Enter",
                    theme: "dracula"
                }))

            }
        }
    }

    if (flag_team2) {
        for (let i = 0; i < lobby.length; i++) {
            if (lobby[i].team_id == team2) {
                const team_id = lobby[i].team_id
                const player_id = lobby[i].player_id
                const block_id = codeblocks.length + 1
                const element_id = merge_codeblock_id(team_id, player_id, block_id)
                codeblocks.push({ element_id: element_id, team_id: team_id, player_id: player_id, block_id: block_id, code: '' })

                const codeblock_div = document.createElement('div')
                const codeblock_textarea = document.createElement('textarea')

                codeblock_div.className = 'codeblock'
                codeblock_textarea.id = element_id

                codeblock_div.appendChild(codeblock_textarea)

                document.getElementById(team2).appendChild(codeblock_div)

                codemirror_objects.push(CodeMirror.fromTextArea(document.getElementById(element_id), {
                    matchBrackets: true,
                    continueComments: "Enter",
                    theme: "dracula"
                }))
            }
        }
    }


}


