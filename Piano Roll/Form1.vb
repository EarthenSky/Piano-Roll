Imports System.IO

'1 is start of note, 2 is body, 3 is end, 0 is no note.  All else is just a temp colour.
Enum BlockValue
    Empty = 0
    Start = 1
    Body = 2
    Close = 3  'End
    StartColour = 4
    BodyColour = 5
    CloseColour = 6
End Enum

'Holds note values in hz
Enum Note
    B7 = 3951
    B6 = 1976
    Ash6 = 1865
    A6 = 1760
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

    Private shtBPM As Short = 120

    Private shtXIndex As Short = 0
    Private shtYIndex As Short = 0

    Private blnIsStartMoving As Boolean = False
    Private blnIsCloseMoving As Boolean = False
    Private blnIsBodyMoving As Boolean = False

    Private pntLastMousePos As Point = New Point(0, 0)
    Private lstLastNote As List(Of BlockValue) = New List(Of BlockValue)

    Private currentFileDirectory As String = Directory.GetCurrentDirectory.Remove(Directory.GetCurrentDirectory.IndexOf("\bin\Debug"), 10) + "\"

    Private imgBackgroundTiles As Image = Image.FromFile(currentFileDirectory & "Grid of tiles.png")
    Private imgRedTile As Image = Image.FromFile(currentFileDirectory & "RedTile.png")
    Private imgBrownTile As Image = Image.FromFile(currentFileDirectory & "BrownTile.png")
    Private imgTealTile As Image = Image.FromFile(currentFileDirectory & "TealTile.png")
    Private imgTintedRedTile As Image = Image.FromFile(currentFileDirectory & "TintedRedTile.png")
    Private imgTintedBrownTile As Image = Image.FromFile(currentFileDirectory & "TintedBrownTile.png")
    Private imgTintedTealTile As Image = Image.FromFile(currentFileDirectory & "TintedTealTile.png")

    Private aryNoteValue(511, 59) As BlockValue 'Holds values of notes

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PaintNotes(ByVal o As Object, ByVal e As PaintEventArgs) Handles pbxGrid.Paint
        e.Graphics.DrawImage(Image.FromFile(currentFileDirectory & "Grid of tiles.png"), Point.Empty)

        'Problem? VVV  Fixed... i think.
        For xPos As Integer = 0 To aryNoteValue.GetLength(0) - 1 'goes through each value and sets the colour
            For yPos As Integer = 0 To aryNoteValue.GetLength(1) - 1
                If aryNoteValue(xPos, yPos) = BlockValue.Empty Then
                    Continue For 'TOME: Arbitrary but nice to see for parsing through code.
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.Start Then
                    e.Graphics.DrawImage(imgRedTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.Body Then
                    e.Graphics.DrawImage(imgBrownTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.Close Then
                    e.Graphics.DrawImage(imgTealTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.StartColour Then
                    e.Graphics.DrawImage(imgTintedRedTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.BodyColour Then
                    e.Graphics.DrawImage(imgTintedBrownTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                ElseIf aryNoteValue(xPos, yPos) = BlockValue.CloseColour Then
                    e.Graphics.DrawImage(imgTintedTealTile, New Point(xPos * shtBlockSizeX, yPos * shtBlockSizeY))
                End If
            Next yPos
        Next xPos
    End Sub

    Private Sub MouseUpClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pbxGrid.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            shtXIndex = (e.X + 0) \ shtBlockSizeX
            shtYIndex = (e.Y + 0) \ shtBlockSizeY

            If blnIsStartMoving = True Then
                aryNoteValue(pntLastMousePos.X, pntLastMousePos.Y) = BlockValue.Start
                blnIsStartMoving = False
                lstLastNote.Clear()  'Clear the list for next movement
            ElseIf blnIsBodyMoving = True Then

            ElseIf blnIsCloseMoving = True Then

            End If
            Refresh()
        End If
    End Sub

    Private Sub MouseDownClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles pbxGrid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            shtXIndex = (e.X + 0) \ shtBlockSizeX
            shtYIndex = (e.Y + 0) \ shtBlockSizeY
            Me.Text = "X: " & shtXIndex & " Y: " & shtYIndex

            'tests
            Select Case (shtYIndex)
                Case 0
                    PlaySound(Note.Csh5, 50)
                Case 1
                    PlaySound(Note.C5, 50)
                Case 2
                    PlaySound(Note.B4, 50)
                Case 3
                    PlaySound(Note.Ash4, 50)
                Case 4
                    PlaySound(Note.A4, 50)
                Case 5
                Case 6
                Case 7
                Case 8
                Case 9
                Case 10
                Case 11
                Case 12

            End Select

            If aryNoteValue(shtXIndex, shtYIndex) = BlockValue.Empty Then
                Me.tbxDebugLog.Text += " 0"
                'places a note at the specific place clicked
                FindAndDeleteNote(shtXIndex + 1, shtYIndex)
                aryNoteValue(shtXIndex + 1, shtYIndex) = BlockValue.Close
                aryNoteValue(shtXIndex, shtYIndex) = BlockValue.Start

            ElseIf aryNoteValue(shtXIndex, shtYIndex) = BlockValue.Start Then
                If blnIsStartMoving = False Then
                    Me.tbxDebugLog.Text += " 1"
                    'changes the colour of the start block
                    aryNoteValue(shtXIndex, shtYIndex) = BlockValue.StartColour  'Colour change is so you know you clicked it.
                    pntLastMousePos = New Point(shtXIndex, shtYIndex)

                    Dim shtCurrentPlace As Short = 0
                    While aryNoteValue(shtXIndex + shtCurrentPlace, shtYIndex) <> 3
                        lstLastNote.Add(aryNoteValue(shtXIndex + shtCurrentPlace, shtYIndex))
                        shtCurrentPlace += 1

                    End While
                    lstLastNote.Add(aryNoteValue(shtXIndex + shtCurrentPlace, shtYIndex))
                    blnIsStartMoving = True

                End If
            ElseIf aryNoteValue(shtXIndex, shtYIndex) = BlockValue.Body Then
                If blnIsBodyMoving = False Then
                    Me.tbxDebugLog.Text += " 2"
                    blnIsBodyMoving = True

                End If
            ElseIf aryNoteValue(shtXIndex, shtYIndex) = BlockValue.Close Then
                If blnIsCloseMoving = False Then
                    Me.tbxDebugLog.Text += " 3"
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

                'deletes the 3
                aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                shtCurrentPlace -= 1

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

                While aryNoteValue(xIndex + shtCurrentPlace, yIndex) <> BlockValue.Empty
                    If aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Body OrElse aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.BodyColour Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        shtCurrentPlace += 1

                    ElseIf aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Close OrElse aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.CloseColour Then
                        aryNoteValue(xIndex + shtCurrentPlace, yIndex) = BlockValue.Empty
                        Exit While

                    End If
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
    ''' Plays a sound based on the note and the length
    ''' </summary>
    ''' <param name="length">Input a number like 7/32</param>
    Private Sub PlaySound(ByVal note As Note, ByVal length As Short)
        Console.Beep(note, length)
    End Sub

    Private Sub tmrUpdateTick() Handles tmrUpdate.Tick
        If blnIsStartMoving = True Then
            shtXIndex = (MousePosition.X - (Me.Bounds.Location.X + 23)) \ shtBlockSizeX + 1
            shtYIndex = (MousePosition.Y - (Me.Bounds.Location.Y + 45)) \ shtBlockSizeY + 1
            Me.Text = shtYIndex.ToString & " " & pntLastMousePos.Y.ToString

            If shtXIndex <> pntLastMousePos.X OrElse shtYIndex <> pntLastMousePos.Y Then  'If the cursor has moved from last position
                FindAndDeleteNote(pntLastMousePos.X, pntLastMousePos.Y) 'Deletes the last note.

                Dim shtCurrentPlace As Short = 0
                For Each block As BlockValue In lstLastNote 'places a new note
                    FindAndDeleteNote(shtXIndex + shtCurrentPlace, shtYIndex)  'Makes sure the note is not placed ontop of another.  TODO: this is not the best solution
                    aryNoteValue(shtXIndex + shtCurrentPlace, shtYIndex) = block
                    shtCurrentPlace += 1

                Next block
                pntLastMousePos = New Point(shtXIndex, shtYIndex)
                Refresh()
            End If

        End If
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
