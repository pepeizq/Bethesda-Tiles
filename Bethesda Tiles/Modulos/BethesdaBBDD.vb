Module BethesdaBBDD

    'https://github.com/FriendsOfGalaxy/galaxy-integration-bethesda/blob/master/betty/game_cache.py

    Public Function Listado()
        Dim lista As New List(Of BethesdaBBDDEntrada) From {
            New BethesdaBBDDEntrada("Arx Fatalis", "41", "1700", Nothing),
            New BethesdaBBDDEntrada("Call of Cthulhu: Dark Corners of the Earth", "35", "22340", Nothing),
            New BethesdaBBDDEntrada("Dishonored 2", "18", "403640", Nothing),
            New BethesdaBBDDEntrada("Dishonored: Death of the Outsider", "19", "614570", Nothing),
            New BethesdaBBDDEntrada("DOOM 3: BFG Edition", "40", "208200", Nothing),
            New BethesdaBBDDEntrada("DOOM Eternal", "51", "782330", Nothing),
            New BethesdaBBDDEntrada("DOOM II", "25", "2300", Nothing),
            New BethesdaBBDDEntrada("Fallout 2: A Post Nuclear Role Playing Game", "22", "38410", Nothing),
            New BethesdaBBDDEntrada("Fallout 3 Game of the Year Edition", "33", "22370", Nothing),
            New BethesdaBBDDEntrada("Fallout 76", "20", "1151340", Nothing),
            New BethesdaBBDDEntrada("Fallout New Vegas Ultimate Edition", "34", "22380", Nothing),
            New BethesdaBBDDEntrada("Fallout Shelter", "8", "588430", Nothing),
            New BethesdaBBDDEntrada("Fallout Tactics: Brotherhood of Steel", "23", "38420", Nothing),
            New BethesdaBBDDEntrada("Fallout: A Post Nuclear Role Playing Game", "21", "38400", Nothing),
            New BethesdaBBDDEntrada("Quake", "36", "2310", Nothing),
            New BethesdaBBDDEntrada("Quake Champions", "11", "611500", Nothing),
            New BethesdaBBDDEntrada("Quake II", "37", "2320", Nothing),
            New BethesdaBBDDEntrada("Quake III Arena", "38", "2200", Nothing),
            New BethesdaBBDDEntrada("RAGE 2", "45", "548570", Nothing),
            New BethesdaBBDDEntrada("Return to Castle Wolfenstein", "27", "9010", Nothing),
            New BethesdaBBDDEntrada("The Elder Scrolls II: Daggerfall", "64", Nothing, Nothing),
            New BethesdaBBDDEntrada("The Elder Scrolls III: Morrowind Game of the Year Edition", "31", "22320", Nothing),
            New BethesdaBBDDEntrada("The Elder Scrolls IV: Oblivion Game of the Year Edition", "32", "22330", Nothing),
            New BethesdaBBDDEntrada("The Elder Scrolls: Arena", "63", Nothing, Nothing),
            New BethesdaBBDDEntrada("The Elder Scrolls: Legends", "5", "364470", Nothing),
            New BethesdaBBDDEntrada("The Evil Within 2", "16", "601430", Nothing),
            New BethesdaBBDDEntrada("The Evil Within", "17", "268050", Nothing),
            New BethesdaBBDDEntrada("Ultimate DOOM", "24", "2280", Nothing),
            New BethesdaBBDDEntrada("Wolfenstein 3D", "26", "2270", Nothing),
            New BethesdaBBDDEntrada("Wolfenstein: Enemy Territory", "39", Nothing, Nothing),
            New BethesdaBBDDEntrada("Wolfenstein II: The New Colossus", "15", "612880", Nothing),
            New BethesdaBBDDEntrada("Wolfenstein: Youngblood", "49", "1056960", New List(Of BethesdaBBDDPaisID) From {New BethesdaBBDDPaisID("DE", "50")})
        }

        Return lista
    End Function

End Module

Public Class BethesdaBBDDEntrada

    Public Titulo As String
    Public IDBethesda As String
    Public IDSteam As String
    Public IDBethesdaOtrosPaises As List(Of BethesdaBBDDPaisID)

    Public Sub New(ByVal titulo As String, ByVal idBethesda As String, ByVal idSteam As String, ByVal idBethesdaOtrosPaises As List(Of BethesdaBBDDPaisID))
        Me.Titulo = titulo
        Me.IDBethesda = idBethesda
        Me.IDSteam = idSteam
        Me.IDBethesdaOtrosPaises = idBethesdaOtrosPaises
    End Sub

End Class

Public Class BethesdaBBDDPaisID

    Public Pais As String
    Public ID As String

    Public Sub New(ByVal pais As String, ByVal id As String)
        Me.Pais = pais
        Me.ID = id
    End Sub

End Class