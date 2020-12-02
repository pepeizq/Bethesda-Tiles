Public Class Tile

    Public Titulo As String
    Public IDBethesda As String
    Public IDSteam As String
    Public Enlace As String

    Public ImagenIcono As String
    Public ImagenLogo As String
    Public ImagenAncha As String
    Public ImagenGrande As String

    Public Sub New(ByVal titulo As String, ByVal idBethesda As String, ByVal idSteam As String, ByVal enlace As String,
                   ByVal imagenIcono As String, ByVal imagenLogo As String, ByVal imagenAncha As String, ByVal imagenGrande As String)
        Me.Titulo = titulo
        Me.IDBethesda = idBethesda
        Me.IDSteam = idSteam
        Me.Enlace = enlace

        Me.ImagenIcono = imagenIcono
        Me.ImagenLogo = imagenLogo
        Me.ImagenAncha = imagenAncha
        Me.ImagenGrande = imagenGrande
    End Sub
End Class
