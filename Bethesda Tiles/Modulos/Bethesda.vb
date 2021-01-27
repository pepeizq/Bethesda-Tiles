Imports Bethesda_Tiles.Configuracion
Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI
Imports Windows.UI.Xaml.Media.Animation

Module Bethesda

    Public anchoColumna As Integer = 180
    Dim dominioImagenes As String = "https://cdn.cloudflare.steamstatic.com"

    Public Async Sub Generar()

        Dim helper As New LocalObjectStorageHelper

        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim gridProgreso As Grid = pagina.FindName("gridProgreso")
        Interfaz.Pestañas.Visibilidad(gridProgreso, Nothing, Nothing)

        Dim pbProgreso As ProgressBar = pagina.FindName("pbProgreso")
        pbProgreso.Value = 0

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")
        tbProgreso.Text = String.Empty

        Cache.Estado(False)
        LimpiezaArchivos.Estado(False)

        Dim gv As AdaptiveGridView = pagina.FindName("gvTiles")
        gv.DesiredWidth = anchoColumna
        gv.Items.Clear()

        Dim listaJuegos As New List(Of Tile)

        If Await helper.FileExistsAsync("juegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")
        End If

        If listaJuegos Is Nothing Then
            listaJuegos = New List(Of Tile)
        End If

        Dim listaBBDD As List(Of BethesdaBBDDEntrada) = BethesdaBBDD.Listado

        Dim i As Integer = 0
        For Each juegoBBDD In listaBBDD
            Dim añadir As Boolean = True
            Dim g As Integer = 0
            While g < listaJuegos.Count
                If listaJuegos(g).IDBethesda = juegoBBDD.IDBethesda Then
                    añadir = False
                End If
                g += 1
            End While

            If añadir = True Then
                Dim imagenPequeña As String = Await Cache.DescargarImagen(Nothing, juegoBBDD.IDBethesda, "pequeña")
                Dim imagenMediana As String = String.Empty
                Dim imagenAncha As String = String.Empty
                Dim imagenGrande As String = String.Empty

                If Not juegoBBDD.IDSteam = Nothing Then
                    Try
                        imagenMediana = Await Cache.DescargarImagen(dominioImagenes + "/steam/apps/" + juegoBBDD.IDSteam + "/logo.png", juegoBBDD.IDSteam, "logo")
                    Catch ex As Exception

                    End Try

                    Try
                        imagenAncha = Await Cache.DescargarImagen(dominioImagenes + "/steam/apps/" + juegoBBDD.IDSteam + "/header.jpg", juegoBBDD.IDSteam, "ancha")
                    Catch ex As Exception

                    End Try

                    Try
                        imagenGrande = Await Cache.DescargarImagen(dominioImagenes + "/steam/apps/" + juegoBBDD.IDSteam + "/library_600x900.jpg", juegoBBDD.IDSteam, "grande")
                    Catch ex As Exception

                    End Try

                    If imagenGrande = String.Empty Then
                        Try
                            imagenGrande = Await Cache.DescargarImagen(dominioImagenes + "/steam/apps/" + juegoBBDD.IDSteam + "/capsule_616x353.jpg", juegoBBDD.IDSteam, "grande")
                        Catch ex As Exception

                        End Try
                    End If
                Else
                    imagenMediana = Await Cache.DescargarImagen(Nothing, juegoBBDD.IDBethesda, "mediana")
                    imagenAncha = Await Cache.DescargarImagen(Nothing, juegoBBDD.IDBethesda, "ancha")
                    imagenGrande = Await Cache.DescargarImagen(Nothing, juegoBBDD.IDBethesda, "grande")
                End If

                Dim enlace As String = "bethesdanet://run/" + juegoBBDD.IDBethesda
                Dim pais As New Windows.Globalization.GeographicRegion

                If Not juegoBBDD.IDBethesdaOtrosPaises Is Nothing Then
                    If juegoBBDD.IDBethesdaOtrosPaises.Count > 0 Then
                        For Each paisID In juegoBBDD.IDBethesdaOtrosPaises
                            If pais.CodeTwoLetter.ToUpper = paisID.Pais.ToUpper Then
                                enlace = "bethesdanet://run/" + paisID.ID
                            End If
                        Next
                    End If
                End If

                Dim juego As New Tile(juegoBBDD.Titulo, juegoBBDD.IDBethesda, juegoBBDD.IDSteam, enlace, imagenPequeña, imagenMediana, imagenAncha, imagenGrande)

                listaJuegos.Add(juego)
            End If

            pbProgreso.Value = CInt((100 / listaBBDD.Count) * i)
            tbProgreso.Text = i.ToString + "/" + listaBBDD.Count.ToString
            i += 1
        Next

        Try
            Await helper.SaveFileAsync(Of List(Of Tile))("juegos", listaJuegos)
        Catch ex As Exception

        End Try

        Dim gridJuegos As Grid = pagina.FindName("gridJuegos")
        Interfaz.Pestañas.Visibilidad(gridJuegos, recursos.GetString("Games"), Nothing)

        'Dim textoClipboard As String = String.Empty

        If Not listaJuegos Is Nothing Then
            If listaJuegos.Count > 0 Then
                listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

                gv.Items.Clear()

                For Each juego In listaJuegos
                    'textoClipboard = textoClipboard + "[tr][td]" + juego.Titulo + "[/td][td]" + juego.IDBethesda + "[/td][/tr]"
                    BotonEstilo(juego, gv)
                Next
            End If
        End If

        'Dim datos As New DataTransfer.DataPackage
        'datos.SetText(textoClipboard)
        'DataTransfer.Clipboard.SetContent(datos)

        Cache.Estado(True)
        LimpiezaArchivos.Estado(True)

    End Sub

    Public Sub BotonEstilo(juego As Tile, gv As GridView)

        Dim panel As New DropShadowPanel With {
            .Margin = New Thickness(10, 10, 10, 10),
            .ShadowOpacity = 0.9,
            .BlurRadius = 10,
            .MaxWidth = anchoColumna + 20,
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center
        }

        Dim boton As New Button

        Dim imagen As New ImageEx With {
            .Source = juego.ImagenGrande,
            .IsCacheEnabled = True,
            .Stretch = Stretch.UniformToFill,
            .Padding = New Thickness(0, 0, 0, 0),
            .HorizontalAlignment = HorizontalAlignment.Center,
            .VerticalAlignment = VerticalAlignment.Center,
            .EnableLazyLoading = True
        }

        boton.Tag = juego
        boton.Content = imagen
        boton.Padding = New Thickness(0, 0, 0, 0)
        boton.Background = New SolidColorBrush(Colors.Transparent)

        panel.Content = boton

        Dim tbToolTip As TextBlock = New TextBlock With {
            .Text = juego.Titulo,
            .FontSize = 16,
            .TextWrapping = TextWrapping.Wrap
        }

        ToolTipService.SetToolTip(boton, tbToolTip)
        ToolTipService.SetPlacement(boton, PlacementMode.Mouse)

        AddHandler boton.Click, AddressOf BotonTile_Click
        AddHandler boton.PointerEntered, AddressOf Interfaz.Entra_Boton_Imagen
        AddHandler boton.PointerExited, AddressOf Interfaz.Sale_Boton_Imagen

        gv.Items.Add(panel)

    End Sub

    Private Async Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Trial.Detectar()
        Interfaz.AñadirTile.ResetearValores()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonJuego As Button = e.OriginalSource
        Dim juego As Tile = botonJuego.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = juego.ImagenAncha

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

        Dim gridAñadirTile As Grid = pagina.FindName("gridAñadirTile")
        Interfaz.Pestañas.Visibilidad(gridAñadirTile, juego.Titulo, Nothing)

        '---------------------------------------------

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionJuego", botonJuego)
        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionJuego")

        If Not animacion Is Nothing Then
            animacion.Configuration = New BasicConnectedAnimationConfiguration
            animacion.TryStart(gridAñadirTile)
        End If

        '---------------------------------------------

        Dim tbImagenTituloTextoTileAncha As TextBox = pagina.FindName("tbImagenTituloTextoTileAncha")
        tbImagenTituloTextoTileAncha.Text = juego.Titulo
        tbImagenTituloTextoTileAncha.Tag = juego.Titulo

        Dim tbImagenTituloTextoTileGrande As TextBox = pagina.FindName("tbImagenTituloTextoTileGrande")
        tbImagenTituloTextoTileGrande.Text = juego.Titulo
        tbImagenTituloTextoTileGrande.Tag = juego.Titulo

        '---------------------------------------------

        Dim imagenPequeña As ImageEx = pagina.FindName("imagenTilePequeña")
        imagenPequeña.Source = Nothing

        Dim imagenMediana As ImageEx = pagina.FindName("imagenTileMediana")
        imagenMediana.Source = Nothing

        Dim imagenAncha As ImageEx = pagina.FindName("imagenTileAncha")
        imagenAncha.Source = Nothing

        Dim imagenGrande As ImageEx = pagina.FindName("imagenTileGrande")
        imagenGrande.Source = Nothing

        If Not juego.IDSteam = Nothing Then
            Try
                juego.ImagenIcono = Await Cache.DescargarImagen(Await SacarIcono(juego.IDBethesda), juego.IDBethesda, "icono")
            Catch ex As Exception

            End Try
        End If

        If Not juego.ImagenIcono = Nothing Then
            imagenPequeña.Source = juego.ImagenIcono
            imagenPequeña.Tag = juego.ImagenIcono
        End If

        If Not juego.ImagenAncha = Nothing Then
            If Not juego.ImagenLogo = Nothing Then
                imagenMediana.Source = juego.ImagenLogo
                imagenMediana.Tag = juego.ImagenLogo
            Else
                imagenMediana.Source = juego.ImagenAncha
                imagenMediana.Tag = juego.ImagenAncha
            End If

            imagenAncha.Source = juego.ImagenAncha
            imagenAncha.Tag = juego.ImagenAncha
        End If

        If Not juego.ImagenGrande = Nothing Then
            imagenGrande.Source = juego.ImagenGrande
            imagenGrande.Tag = juego.ImagenGrande
        End If

    End Sub

    Public Async Function SacarIcono(id As String) As Task(Of String)

        Dim helper As New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("juegos") = True Then
            Dim listaJuegos As List(Of Tile) = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")

            For Each juego In listaJuegos
                If id = juego.IDBethesda Then
                    If Not juego.ImagenIcono = Nothing Then
                        Return juego.ImagenIcono
                    End If
                End If
            Next
        End If

        Dim html As String = Await Decompiladores.HttpClient(New Uri("https://store.steampowered.com/app/" + id + "/"))
        Dim urlIcono As String = String.Empty

        If Not html = Nothing Then
            If html.Contains("<div class=" + ChrW(34) + "apphub_AppIcon") Then
                Dim temp, temp2 As String
                Dim int, int2 As Integer

                int = html.IndexOf("<div class=" + ChrW(34) + "apphub_AppIcon")
                temp = html.Remove(0, int)

                int = temp.IndexOf("<img src=")
                temp = temp.Remove(0, int + 10)

                int2 = temp.IndexOf(ChrW(34))
                temp2 = temp.Remove(int2, temp.Length - int2)

                temp2 = temp2.Replace("%CDN_HOST_MEDIA_SSL%", "steamcdn-a.akamaihd.net")

                urlIcono = temp2.Trim
            End If
        End If

        If urlIcono = Nothing Then
            html = Await Decompiladores.HttpClient(New Uri("https://steamdb.info/app/" + id + "/"))

            If Not html = Nothing Then
                If html.Contains("<img class=" + ChrW(34) + "app-icon avatar") Then
                    Dim temp, temp2 As String
                    Dim int, int2 As Integer

                    int = html.IndexOf("<img class=" + ChrW(34) + "app-icon avatar")
                    temp = html.Remove(0, int)

                    int = temp.IndexOf("src=")
                    temp = temp.Remove(0, int + 5)

                    int2 = temp.IndexOf(ChrW(34))
                    temp2 = temp.Remove(int2, temp.Length - int2)

                    urlIcono = temp2.Trim
                End If
            End If
        End If

        If Not urlIcono = String.Empty Then
            If Await helper.FileExistsAsync("juegos") = True Then
                Dim listaJuegos As List(Of Tile) = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")

                For Each juego In listaJuegos
                    If id = juego.IDBethesda Then
                        juego.ImagenIcono = Await Cache.DescargarImagen(urlIcono, id, "icono")
                    End If
                Next

                Await helper.SaveFileAsync(Of List(Of Tile))("juegos", listaJuegos)
            End If
        End If

        Return urlIcono
    End Function

End Module
