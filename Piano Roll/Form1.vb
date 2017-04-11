Imports System.IO
Imports System.Media
Imports System.Threading
Imports WMPLib
Imports NAudio
Imports NAudio.Wave

'1 is start of note, 2 is body, 3 is end, 0 is no note.  All else is just a temp colour.
Public Enum BlockValue
    Empty = 0
    Start = 1
    Body = 2
    Close = 3  'End
    StartColour = 4
    BodyColour = 5
    CloseColour = 6
End Enum

'Holds note values in hz
Public Enum Note
    B7 = 3951
    Ash7 = 3729
    A7 = 3520
    Gsh7 = 3322
    G7 = 3136
    Fsh7 = 2960
    F7 = 2794
    E7 = 2637
    Dsh7 = 2489
    D7 = 2349
    Csh7 = 2217
    C7 = 2093
    B6 = 1976
    Ash6 = 1865
    A6 = 1760
    Gsh6 = 1661
    G6 = 1568
    Fsh6 = 1480
    F6 = 1397
    B5 = 988
    Gsh5 = 831
    G5 = 784
    Fsh5 = 740
    F5 = 698
    E5 = 659
    Dsh5 = 622
    D5 = 587
    Csh5 = 554
    C5 = 523
    B4 = 494
    Ash4 = 466
    A4 = 440
    B3 = 247
    B2 = 123
End Enum

Public Class Form1
    'Piano Roll 
    'Gabe Stang
    '
    'Play a piano on the piano roll

    Const shtBlockSizeX As Short = 8
    Const shtBlockSizeY As Short = 8

    Private shtCurrentNotePlayIndex = 0
    Private shtBPM As Short = 120

    Private shtMouseX As Short = 0
    Private shtMouseY As Short = 0

    Private shtGridMovement As Short = 0

    Private blnIsStartMoving As Boolean = False
    Private blnIsCloseMoving As Boolean = False
    Private blnIsBodyMoving As Boolean = False

    Private pntLastMousePos As Point = New Point(0, 0)
    Private pntLastNoteStartPos As Point = New Point(0, 0)
    Private lstLastNote As List(Of BlockValue) = New List(Of BlockValue)

    Private currentFileDirectory As String = Directory.GetCurrentDirectory.Remove(Directory.GetCurrentDirectory.IndexOf("\bin\Debug"), 10) + "\"

    Private imgBackgroundTiles As Image = Image.FromFile(currentFileDirectory & "Grid of tiles.png")
    Private imgRedTile As Image = Image.FromFile(currentFileDirectory & "RedTile.png")
    Private imgBrownTile As Image = Image.FromFile(currentFileDirectory & "BrownTile.png")
    Private imgTealTile As Image = Image.FromFile(currentFileDirectory & "TealTile.png")
    Private imgTintedRedTile As Image = Image.FromFile(currentFileDirectory & "TintedRedTile.png")
    Private imgTintedBrownTile As Image = Image.FromFile(currentFileDirectory & "TintedBrownTile.png")
    Private imgTintedTealTile As Image = Image.FromFile(currentFileDirectory & "TintedTealTile.png")
    Private imgDarkDarkGreyTile As Image = Image.FromFile(currentFileDirectory & "DarkDarkGreyTile.png")
    Private imgBarMarker As Image = Image.FromFile(currentFileDirectory & "BarMarker.png")
    Private imgSmallBarMarker As Image = Image.FromFile(currentFileDirectory & "BarMarkerSmall.png")
    Private imgPlaceBarMarker As Image = Image.FromFile(currentFileDirectory & "PlaceMarkerRed.png")

    Private aryNoteValue(511, 59) As BlockValue 'Holds values of notes

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub

    Private Sub PaintNotes(ByVal o As Object, ByVal e As PaintEventArgs) Handles pbxGrid.Paint
        e.Graphics.DrawImage(Image.FromFile(currentFileDirectory & "Grid of tiles.png"), Point.Empty)

        'Problem? VVV  Fixed... i think.
        For xPos As Short = 0 To aryNoteValue.GetLength(0) - 1 'goes through each value and sets the colour
            For yPos As Short = 0 To aryNoteValue.GetLength(1) - 1
                If aryNoteValue(xPos, yPos) = BlockValue.Empty Then
                    Continue For 'TOME: Arbitrary but nice to see for parsing through code.
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.Start Then
                    e.Graphics.DrawImage(imgRedTile, New Point((xPos - shtGridMovement) * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.Body Then
                    e.Graphics.DrawImage(imgBrownTile, New Point((xPos - shtGridMovement) * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.Close Then
                    e.Graphics.DrawImage(imgTealTile, New Point((xPos - shtGridMovement) * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.StartColour Then
                    e.Graphics.DrawImage(imgTintedRedTile, New Point((xPos - shtGridMovement) * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.BodyColour Then
                    e.Graphics.DrawImage(imgTintedBrownTile, New Point((xPos - shtGridMovement) * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.CloseColour Then
                    e.Graphics.DrawImage(imgTintedTealTile, New Point((xPos - shtGridMovement) * shtBlockSizeX, yPos * shtBlockSizeY))
                End If
            Next yPos
        Next xPos

        If -shtGridMovement > 0 Then
            For gridX As Short = 0 To -shtGridMovement - 1
                For yPos As Short = 0 To aryNoteValue.GetLength(1) - 1
                    e.Graphics.DrawImage(imgDarkDarkGreyTile, New Point(gridX * shtBlockSizeX, yPos * shtBlockSizeY))
                Next yPos
            Next gridX
        End If

        e.Graphics.DrawImage(imgPlaceBarMarker, New Point((shtCurrentNotePlayIndex - shtGridMovement) * shtBlockSizeX, 0))

    End Sub

    Private Sub PaintBarMarkers(ByVal o As Object, ByVal e As PaintEventArgs) Handles pbxBarMarker.Paint
        For index As Short = shtGridMovement To aryNoteValue.GetLength(0) - 1
            If index Mod 8 = 0 Then
                If index Mod 32 = 0 Then
                    e.Graphics.DrawImage(imgBarMarker, New Point((-shtGridMovement + index) * shtBlockSizeX, 0))
                Else
                    e.Graphics.DrawImage(imgSmallBarMarker, New Point((-shtGridMovement + index) * shtBlockSizeX, 4))
                End If

            End If
        Next
    End Sub

    Private Sub MouseUpClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pbxGrid.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            shtMouseX = (e.X - shtGridMovement) \ shtBlockSizeX
            shtMouseY = (e.Y + 0) \ shtBlockSizeY

            If blnIsStartMoving = True Then
                aryNoteValue(pntLastMousePos.X, pntLastMousePos.Y) = BlockValue.Start
                pntLastMousePos = Point.Empty
                pntLastNoteStartPos = Point.Empty
                lstLastNote.Clear()  'Clear the list for next movement (needed?)
                blnIsStartMoving = False
            ElseIf blnIsBodyMoving = True Then
                pntLastMousePos = Point.Empty
                pntLastNoteStartPos = Point.Empty
                lstLastNote.Clear()  'Clear the list for next movement (needed?)
                blnIsBodyMoving = False
            ElseIf blnIsCloseMoving = True Then
                aryNoteValue(pntLastMousePos.X, pntLastMousePos.Y) = BlockValue.Close
                pntLastMousePos = Point.Empty
                pntLastNoteStartPos = Point.Empty
                lstLastNote.Clear()  'Clear the list for next movement (needed?)
                blnIsCloseMoving = False
            End If
            Refresh()

        End If
    End Sub

    Private Sub MouseDownClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pbxGrid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            shtMouseX = (e.X) \ shtBlockSizeX + shtGridMovement
            shtMouseY = (e.Y + 0) \ shtBlockSizeY
            Me.Text = "X: " & shtMouseX & " Y: " & shtMouseY

            If shtMouseX < 0 OrElse shtMouseX > 510 Then 'TODO: CLICK SIDEWAYS 400 TIMES
                Exit Sub
            End If

            If aryNoteValue(shtMouseX, shtMouseY) = BlockValue.Empty Then
                'places a note at the specific place clicked
                FindAndDeleteNote(shtMouseX + 1, shtMouseY)
                aryNoteValue(shtMouseX + 1, shtMouseY) = BlockValue.Close
                aryNoteValue(shtMouseX, shtMouseY) = BlockValue.Start

            ElseIf aryNoteValue(shtMouseX, shtMouseY) = BlockValue.Start Then  'Clicking on the start block
                If blnIsStartMoving = False Then
                    'changes the colour of the start block
                    aryNoteValue(shtMouseX, shtMouseY) = BlockValue.StartColour  'Colour change is so you know you clicked it.
                    pntLastMousePos = New Point(shtMouseX, shtMouseY)  'Set last position

                    Dim shtCurrentPlace As Short = 0
                    While aryNoteValue(shtMouseX + shtCurrentPlace, shtMouseY) <> BlockValue.Close
                        lstLastNote.Add(aryNoteValue(shtMouseX + shtCurrentPlace, shtMouseY))
                        shtCurrentPlace += 1

                    End While
                    lstLastNote.Add(aryNoteValue(shtMouseX + shtCurrentPlace, shtMouseY))
                    blnIsStartMoving = True

                End If
            ElseIf aryNoteValue(shtMouseX, shtMouseY) = BlockValue.Body Then  'Clicking on the middle block
                If blnIsBodyMoving = False Then
                    Dim indexUp As Short = 0
                    While aryNoteValue(shtMouseX + indexUp, shtMouseY) <> BlockValue.Start AndAlso _
                          aryNoteValue(shtMouseX + indexUp, shtMouseY) <> BlockValue.StartColour  'Finds the start block
                        indexUp -= 1
                    End While
                    pntLastNoteStartPos = New Point(shtMouseX + indexUp, shtMouseY)
                    Dim shtCurrentPlace As Short = 0
                    While aryNoteValue(pntLastNoteStartPos.X + shtCurrentPlace, shtMouseY) <> BlockValue.Close
                        lstLastNote.Add(aryNoteValue(pntLastNoteStartPos.X + shtCurrentPlace, shtMouseY))
                        shtCurrentPlace += 1

                    End While
                    lstLastNote.Add(aryNoteValue(pntLastNoteStartPos.X + shtCurrentPlace, shtMouseY))
                    pntLastMousePos = New Point(shtMouseX, shtMouseY)
                    blnIsBodyMoving = True

                End If
            ElseIf aryNoteValue(shtMouseX, shtMouseY) = BlockValue.Close Then  'Clicking on the end block (close block)
                If blnIsCloseMoving = False Then
                    aryNoteValue(shtMouseX, shtMouseY) = BlockValue.CloseColour  'Colour change is so you know you clicked it.
                    pntLastMousePos = New Point(shtMouseX, shtMouseY)  'Set last position
                    Dim indexUp As Short = 0
                    While aryNoteValue(shtMouseX + indexUp, shtMouseY) <> BlockValue.Start AndAlso _
                          aryNoteValue(shtMouseX + indexUp, shtMouseY) <> BlockValue.StartColour  'Finds the start block
                        indexUp -= 1
                    End While
                    pntLastNoteStartPos = New Point(shtMouseX + indexUp, shtMouseY)
                    blnIsCloseMoving = True

                End If
            End If
            Refresh()
        End If
    End Sub

    ''' <summary>
    ''' Looks for a note in the containing space and deletes all reference of it.
    ''' </summary>
    Private Sub FindAndDeleteNote(ByVal xIndex As Short, ByVal yIndex As Short)
        Dim shtCurrentPlace As Short = 0
        If aryNoteValue(xIndex, yIndex) <> BlockValue.Empty Then
            'check down the row.  If it is a part of a note, delete it.  Stop when it hits the end of the note.

            If aryNoteValue(xIndex, yIndex) = BlockValue.Start OrElse aryNoteValue(xIndex, yIndex) = BlockValue.StartColour Then
                While aryNoteValue(xIndex + shtCurrentPlace, yIndex) <> BlockValue.Close AndAlso aryNoteValue(xIndex + shtCurrentPlace, yIndex) <> BlockValue.CloseColour
                    aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                    shtCurrentPlace += 1

                End While
                aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty

            ElseIf aryNoteValue(xIndex, yIndex) = BlockValue.Body OrElse aryNoteValue(xIndex, yIndex) = BlockValue.BodyColour Then
                'check up the row.  If it is a part of a note, delete it.  
                'When it hits the start of the note go back to the middle and start down the note until it hits the end of the note. 

                While aryNoteValue(xIndex + shtCurrentPlace, yIndex) <> BlockValue.Empty
                    If aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Body OrElse aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.BodyColour Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        shtCurrentPlace -= 1

                    ElseIf aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Start OrElse aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.StartColour Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        shtCurrentPlace = 0
                        Exit While

                    End If
                End While

                While True
                    If aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Body OrElse aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.BodyColour Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty

                    ElseIf aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Close OrElse aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.CloseColour Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        Exit While

                    End If
                    shtCurrentPlace += 1
                End While

            ElseIf aryNoteValue(xIndex, yIndex) = BlockValue.Close Then
                'Check up the row.  If it is a part of a note, delete it.  When it hits the start of the note stop. 

                'deletes the 3
                aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                shtCurrentPlace -= 1

                While aryNoteValue(xIndex + shtCurrentPlace, yIndex) <> BlockValue.Empty
                    If aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Body Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        shtCurrentPlace -= 1

                    ElseIf aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Start Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        shtCurrentPlace = 0
                        Exit While

                    End If
                End While
            End If
        End If
    End Sub

    ''' <summary>
    ''' Finds the length of a note in x/32 bars.
    ''' </summary>
    ''' <returns>returns -1 if no note is found.</returns>
    Private Function FindNoteLenth(ByVal xIndex As Short, ByVal yIndex As Short) As Short
        If aryNoteValue(xIndex, yIndex) <> BlockValue.Empty Then
            Dim index As Short = 0
            Dim indexUp As Short = 0
            While aryNoteValue(xIndex + index + indexUp, yIndex) <> BlockValue.Start AndAlso _
                  aryNoteValue(xIndex + index + indexUp, yIndex) <> BlockValue.StartColour  'Finds the start block
                indexUp -= 1
            End While
            While aryNoteValue(xIndex + index + indexUp, yIndex) <> BlockValue.Empty
                index += 1
            End While

            Return index
        End If

        Return -1  'No note was found
    End Function

    ''' <summary>
    ''' Plays a sound based on the note and the length
    ''' </summary>
    ''' <param name="length">Input a number like 7/32</param>
    Private Sub PlaySound(ByVal note As Note, ByVal length As Short)
        'Console.Beep(note, length)
        'shtCurrentNotePlayIndex += length / 8 / 60 'tpdo: this is not good working
        Beep.Beep(500, note, length)
    End Sub

    Private Sub tmrUpdateTick() Handles tmrUpdate.Tick
        If blnIsStartMoving = True Then  'Run while holding the note
            shtMouseX = (MousePosition.X - (Me.Bounds.Location.X + 23)) \ shtBlockSizeX + 1 + shtGridMovement
            shtMouseY = (MousePosition.Y - (Me.Bounds.Location.Y + 45)) \ shtBlockSizeY + 1

            If (shtMouseX <> pntLastMousePos.X OrElse shtMouseY <> pntLastMousePos.Y) And shtMouseX >= 0 Then  'If the cursor has moved from last position
                FindAndDeleteNote(pntLastMousePos.X, pntLastMousePos.Y) 'Deletes the last note.

                Dim shtCurrentPlace As Short = 0
                For Each block As BlockValue In lstLastNote 'places a new note
                    FindAndDeleteNote(shtMouseX + shtCurrentPlace, shtMouseY)  'Makes sure the note is not placed ontop of another.  TODO: this is not the best solution
                    aryNoteValue(shtMouseX + shtCurrentPlace, shtMouseY) = block
                    shtCurrentPlace += 1

                Next block
                pntLastMousePos = New Point(shtMouseX, shtMouseY)
                Refresh()

            End If
        ElseIf blnIsCloseMoving = True Then  'Run while extending the note
            shtMouseX = (MousePosition.X - (Me.Bounds.Location.X + 23)) \ shtBlockSizeX + 1 + shtGridMovement
            shtMouseY = (MousePosition.Y - (Me.Bounds.Location.Y + 45)) \ shtBlockSizeY + 1

            If shtMouseX <> pntLastMousePos.X Then  'If the cursor has moved from last position in the x axis
                If shtMouseX > pntLastNoteStartPos.X Then
                    If shtMouseX > pntLastMousePos.X Then  'If the note is getting bigger
                        Dim shtCurrentPlace As Short = 0
                        While pntLastMousePos.X + shtCurrentPlace <= shtMouseX - 1
                            aryNoteValue(pntLastMousePos.X + shtCurrentPlace, pntLastMousePos.Y) = BlockValue.Body
                            shtCurrentPlace += 1
                        End While

                        aryNoteValue(pntLastMousePos.X + shtCurrentPlace, pntLastMousePos.Y) = BlockValue.CloseColour
                        pntLastMousePos = New Point(pntLastMousePos.X + shtCurrentPlace, pntLastMousePos.Y)

                    ElseIf shtMouseX < pntLastMousePos.X Then  'If the note is getting smaller
                        Dim shtCurrentPlace As Short = 0
                        While pntLastMousePos.X - shtCurrentPlace >= shtMouseX + 1
                            aryNoteValue(pntLastMousePos.X - shtCurrentPlace, pntLastMousePos.Y) = BlockValue.Empty
                            shtCurrentPlace += 1
                        End While

                        aryNoteValue(pntLastMousePos.X - shtCurrentPlace, pntLastMousePos.Y) = BlockValue.CloseColour
                        pntLastMousePos = New Point(pntLastMousePos.X - shtCurrentPlace, pntLastMousePos.Y)

                    End If
                End If
                Refresh()
            End If
        ElseIf blnIsBodyMoving = True Then
            shtMouseX = (MousePosition.X - (Me.Bounds.Location.X + 23)) \ shtBlockSizeX + 1 + shtGridMovement  'Update position
            shtMouseY = (MousePosition.Y - (Me.Bounds.Location.Y + 45)) \ shtBlockSizeY + 1
            If shtMouseY <> pntLastMousePos.Y Then  'If the cursor has moved from last position
                FindAndDeleteNote(pntLastMousePos.X, pntLastMousePos.Y) 'Deletes the last note.

                Dim shtCurrentPlace As Short = 0
                For Each block As BlockValue In lstLastNote 'places a new note
                    FindAndDeleteNote(shtMouseX + shtCurrentPlace, shtMouseY)  'Makes sure the note is not placed ontop of another.  TODO: this is not the best solution
                    aryNoteValue(pntLastNoteStartPos.X + shtCurrentPlace, shtMouseY) = block
                    shtCurrentPlace += 1

                Next block
                pntLastMousePos = New Point(pntLastMousePos.X, shtMouseY)
                Refresh()

            End If
        End If
    End Sub

    Private Sub KeyInput(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            shtMouseX = (MousePosition.X - (Me.Bounds.Location.X + 23)) \ shtBlockSizeX + 1 + shtGridMovement
            shtMouseY = (MousePosition.Y - (Me.Bounds.Location.Y + 45)) \ shtBlockSizeY + 1

            FindAndDeleteNote(shtMouseX, shtMouseY)

            blnIsStartMoving = False
            blnIsCloseMoving = False
            blnIsBodyMoving = False

            Refresh()
        ElseIf e.KeyCode = Keys.F Then
            shtMouseX = (MousePosition.X - (Me.Bounds.Location.X + 23)) \ shtBlockSizeX + 1 + shtGridMovement
            shtMouseY = (MousePosition.Y - (Me.Bounds.Location.Y + 45)) \ shtBlockSizeY + 1
            tbxDebugLog.Text = FindNoteLenth(shtMouseX, shtMouseY)
        End If
    End Sub

    Private Sub btnRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRight.Click
        shtGridMovement += 1
        Refresh()
    End Sub

    Private Sub btnLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        shtGridMovement -= 1
        Refresh()
    End Sub

    Private Sub btnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlay.Click
        tmrBPMRun.Interval = (1000.0 * 60.0) / shtBPM / 8  'beats per minuite set to bpm then by notes smallness
        tmrBPMRun.Enabled = True
    End Sub

    Public Sub tmrBPMRun_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrBPMRun.Tick
        If shtCurrentNotePlayIndex <= 511 Then
            shtCurrentNotePlayIndex += 1
            For yPos As Short = 0 To aryNoteValue.GetLength(1) - 1
                If aryNoteValue(shtCurrentNotePlayIndex, yPos) = BlockValue.Start Then
                    Select Case (yPos)
                        Case 0
                            PlaySound(Note.B7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 1
                            PlaySound(Note.Ash7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 2
                            PlaySound(Note.A7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 3
                            PlaySound(Note.Gsh7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 4
                            PlaySound(Note.G7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 5
                            PlaySound(Note.Fsh7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 6
                            PlaySound(Note.F7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 7
                            PlaySound(Note.E7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 8
                            PlaySound(Note.Dsh7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 9
                            PlaySound(Note.D7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 10
                            PlaySound(Note.B7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 11
                            PlaySound(Note.Csh7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 12
                            PlaySound(Note.C7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 13
                            PlaySound(Note.B6, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 14
                            PlaySound(Note.Ash6, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 15
                            PlaySound(Note.A6, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 16
                            PlaySound(Note.Gsh6, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 17
                            PlaySound(Note.G6, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 18
                            PlaySound(Note.Fsh7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                        Case 19
                            PlaySound(Note.F7, FindNoteLenth(shtCurrentNotePlayIndex, yPos) * ((1000.0 * 60.0) / shtBPM / 8))
                    End Select
                End If
            Next yPos
        End If
        Refresh()
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click  'Turns timer off then sets the place to run the notes at to 0
        tmrBPMRun.Enabled = False
        shtCurrentNotePlayIndex = 0
        Refresh()
        Beep.EmptyList()
    End Sub

End Class

'//           ▄███▄  /
'//           ██▄██▄ -- <Quack!>
'//       ▄█▄▄▄███▄  \
'//       ██████████
'//       ▀███████▀
'//   ________________________
'//  |CODE DUCK STRIKES AGAIN!|
'//  |________________________|

Public Class Beep

    Private Shared wavReaders As New List(Of IWavePlayer)

    Public Shared Sub EmptyList()
        For Each reader In wavReaders
            If reader IsNot Nothing Then
                reader.[Stop]()
            End If
            If reader IsNot Nothing Then
                reader.Dispose()
                reader = Nothing
            End If
        Next
        wavReaders.Clear()

    End Sub

    Private Shared Sub CloseWaveOut()
        Dim waveOutDevice As IWavePlayer = Nothing
        If waveOutDevice IsNot Nothing Then
            waveOutDevice.[Stop]()
        End If
        If waveOutDevice IsNot Nothing Then
            waveOutDevice.Dispose()
            waveOutDevice = Nothing
        End If
    End Sub

    Private Shared Function CreateInputStream(ByVal mediacontent As Byte()) As WaveStream
        Dim inputStream As WaveChannel32
        Dim ms As Stream = New MemoryStream(mediacontent)
        Dim wavReader As WaveStream = New WaveFileReader(ms)
        inputStream = New WaveChannel32(wavReader)
        Return inputStream
    End Function

    Private Shared Sub PlayStream(ByVal stream As System.IO.MemoryStream)
        Dim myBytes() As Byte = stream.ToArray
        Dim stream2 As WaveStream = CreateInputStream(myBytes)

        wavReaders.Add(New WaveOut())

        wavReaders.Item(wavReaders.Count - 1).Init(stream2)
        wavReaders.Item(wavReaders.Count - 1).Play()

        CloseWaveOut()
    End Sub
    'Private Sub Play_WMP(ByVal ResourceName As String)
    '    Dim SFX As New WindowsMediaPlayer()

    '    Dim ShortPath As String = My.Resources.ResourceManager.BaseName
    '    Dim LongPath As String = IO.Path.GetFullPath(My.Resources.ResourceManager.BaseName)
    '    Dim FullPath As String = LongPath.Substring(0, LongPath.Length - ShortPath.Length - 10) & "Resources\"

    '    SFX.URL = FullPath & ResourceName
    '    SFX.controls.play()
    'End Sub

    'Private Sub Play_WMP2(ByVal FileName As String)
    '    Dim SFX As New WindowsMediaPlayer()
    '    SFX.URL = FileName
    '    SFX.controls.play()
    'End Sub
    Shared Sub Beep(ByVal Amplitude As Integer, _
             ByVal Frequency As Integer, _
             ByVal Duration As Integer)
        Dim A As Double = ((Amplitude * 2 ^ 15) / 1000) - 1
        Dim DeltaFT As Double = 2 * Math.PI * Frequency / 44100

        Dim Samples As Integer = 441 * Duration \ 10
        Dim Bytes As Integer = Samples * 4
        Dim Hdr() As Integer = {&H46464952, 36 + Bytes, &H45564157, _
                                &H20746D66, 16, &H20001, 44100, _
                                 176400, &H100004, &H61746164, Bytes}
        Using MS As New MemoryStream(44 + Bytes)
            Using BW As New BinaryWriter(MS)
                For I As Integer = 0 To Hdr.Length - 1
                    BW.Write(Hdr(I))
                Next
                For T As Integer = 0 To Samples - 1
                    Dim Sample As Short = CShort(A * Math.Sin(DeltaFT * T))
                    BW.Write(Sample)
                    BW.Write(Sample)
                Next
                BW.Flush()
                MS.Seek(0, SeekOrigin.Begin)

                PlayStream(MS)

                'Dim SFX As New WindowsMediaPlayer()
                'SFX.URL = MakeMp3(MS)
                'SFX.controls.play()

                'Using SP As New SoundPlayer(MS)
                '    SP.Play()
                '    'While SP.IsLoadCompleted = True
                '    '    Thread.Sleep(1000)
                '    'End While
                'End Using
            End Using
        End Using
    End Sub
End Class