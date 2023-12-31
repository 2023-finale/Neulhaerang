package com.finale.neulhaerang.ui.app.checklist

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.outlined.Cancel
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.finale.neulhaerang.common.Stat
import com.finale.neulhaerang.ui.R

@Composable
fun CheckListContentInput(
    content: String,
    changeContent: (String) -> Unit,
    clearContent: () -> Unit,
    stat: Stat,
    changeStat: (Stat) -> Unit,
    canModifyStat: Boolean = true,
) {
//    val viewModel = viewModel<CheckListCreationViewModel>()
//
//    val content = viewModel.content
//    val changeContent = viewModel::changeContent
//    val clearContent = viewModel::clearContent

    var showDialog by remember { mutableStateOf(false) }

    Row(verticalAlignment = Alignment.CenterVertically) {
        IconButton(onClick = { showDialog = true }) { StatImage(stat = stat) }
        Spacer(modifier = Modifier.width(8.dp))
        TextField(
            value = content,
            onValueChange = changeContent,
            modifier = Modifier.fillMaxWidth(),
            placeholder = { Text(text = "할 일을 입력해 주세요") },
            trailingIcon = {
                IconButton(onClick = clearContent) {
                    Icon(imageVector = Icons.Outlined.Cancel, contentDescription = "clear")
                }
            },
            singleLine = true,
            colors = TextFieldDefaults.colors(
                errorContainerColor = Color.Transparent,
                focusedContainerColor = Color.Transparent,
                disabledContainerColor = Color.Transparent,
                unfocusedContainerColor = Color.Transparent
            )
        )
        if (showDialog && canModifyStat) {
            StatDialog(onDismiss = { showDialog = false }, stat = stat, changeStat = changeStat)
        }
    }
}

@Composable
fun StatDialog(
    onDismiss: () -> Unit = {},
    stat: Stat,
    changeStat: (Stat) -> Unit,
) {
//    val viewModel = viewModel<CheckListCreationViewModel>()
//
//    val stat = viewModel.stat
//    val changeStat = viewModel::changeStat

    AlertDialog(onDismissRequest = onDismiss, confirmButton = {}, title = {
        Text(text = "스탯을 골라주세요", style = MaterialTheme.typography.bodyLarge)
    }, text = {
        LazyVerticalGrid(
            columns = GridCells.Fixed(2),// contentPadding = PaddingValues(8.dp)
            horizontalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            Stat.values().forEach {
                item {
                    val colors = if (it == stat) ButtonDefaults.buttonColors()
                    else ButtonDefaults.outlinedButtonColors()
                    val border = if (it == stat) null
                    else ButtonDefaults.outlinedButtonBorder

                    Button(
                        onClick = { changeStat(it); onDismiss() },
                        shape = RoundedCornerShape(8.dp),
                        colors = colors,
                        border = border,
                        contentPadding = PaddingValues(0.dp)
                    ) {
                        Text(text = it.statName, style = MaterialTheme.typography.bodyLarge)
                        Spacer(modifier = Modifier.width(4.dp))
                        StatImage(
                            stat = it,
                            modifier = Modifier.size(MaterialTheme.typography.titleLarge.fontSize.value.dp)
                        )
                    }
                }
            }
        }
    })
}

/**
 * 선택된 스탯 이미지 표시하는 composable
 */
@Composable
fun StatImage(stat: Stat, modifier: Modifier = Modifier) {
    val id = when (stat) {
        Stat.GodSang -> R.drawable.godsang
        Stat.Survive -> R.drawable.survive
        Stat.InSsa -> R.drawable.inssa
        Stat.Teunteun -> R.drawable.teunteun
        Stat.GoodIdea -> R.drawable.goodidea
        Stat.Love -> R.drawable.love
    }
    Image(painter = painterResource(id = id), contentDescription = stat.statName, modifier)
}
