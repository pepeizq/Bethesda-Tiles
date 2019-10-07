Module BethesdaBBDD

    'https://github.com/FriendsOfGalaxy/galaxy-integration-bethesda/blob/master/betty/game_cache.py

    'Fallout 76 - 20

    Public Function Listado()
        Dim lista As New List(Of BethesdaBBDDEntrada) From {
            New BethesdaBBDDEntrada("Call of Cthulhu: Dark Corners of the Earth", "35", "22340"),
            New BethesdaBBDDEntrada("Dishonored 2", "18", "403640"),
            New BethesdaBBDDEntrada("Dishonored: Death of the Outsider", "19", "614570"),
            New BethesdaBBDDEntrada("DOOM 3: BFG Edition", "40", "208200"),
            New BethesdaBBDDEntrada("DOOM Eternal", "51", "782330"),
            New BethesdaBBDDEntrada("DOOM II", "25", "2300"),
            New BethesdaBBDDEntrada("Fallout 2: A Post Nuclear Role Playing Game", "22", "38410"),
            New BethesdaBBDDEntrada("Fallout 3 Game of the Year Edition", "33", "22370"),
            New BethesdaBBDDEntrada("Fallout New Vegas Ultimate Edition", "34", "22380"),
            New BethesdaBBDDEntrada("Fallout Shelter", "8", "588430"),
            New BethesdaBBDDEntrada("Fallout Tactics: Brotherhood of Steel", "23", "38420"),
            New BethesdaBBDDEntrada("Fallout: A Post Nuclear Role Playing Game", "21", "38400"),
            New BethesdaBBDDEntrada("Quake", "36", "2310"),
            New BethesdaBBDDEntrada("Quake Champions", "11", "611500"),
            New BethesdaBBDDEntrada("Quake II", "37", "2320"),
            New BethesdaBBDDEntrada("Quake III Arena", "38", "2200"),
            New BethesdaBBDDEntrada("RAGE 2", "45", "548570"),
            New BethesdaBBDDEntrada("Return to Castle Wolfenstein", "27", "9010"),
            New BethesdaBBDDEntrada("The Elder Scrolls III: Morrowind Game of the Year Edition", "31", "22320"),
            New BethesdaBBDDEntrada("The Elder Scrolls IV: Oblivion Game of the Year Edition", "32", "22330"),
            New BethesdaBBDDEntrada("The Elder Scrolls: Legends", "5", "364470"),
            New BethesdaBBDDEntrada("The Evil Within 2", "16", "601430"),
            New BethesdaBBDDEntrada("The Evil Within", "17", "268050"),
            New BethesdaBBDDEntrada("Ultimate DOOM", "24", "2280"),
            New BethesdaBBDDEntrada("Wolfenstein 3D", "26", "2270"),
            New BethesdaBBDDEntrada("Wolfenstein II: The New Colossus", "15", "612880"),
            New BethesdaBBDDEntrada("Wolfenstein: Youngblood", "49", "1056960")
        }

        Return lista
    End Function

End Module

Public Class BethesdaBBDDEntrada

    Public Titulo As String
    Public IDBethesda As String
    Public IDSteam As String

    Public Sub New(ByVal titulo As String, ByVal idBethesda As String, ByVal idSteam As String)
        Me.Titulo = titulo
        Me.IDBethesda = idBethesda
        Me.IDSteam = idSteam
    End Sub

End Class