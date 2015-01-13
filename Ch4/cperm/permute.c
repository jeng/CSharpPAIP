 /* permute - permutes a list of strings passed in as arguments
  * Jeremy English Mon Jan 12 22:51:20 CST 2015
  */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_STACK 0xff
#define UNSET -1

int
isEmpty(int *array, int len)
{
    int i = 0;
    for (; i < len; i++) {
        if (array[i] != UNSET)
            return 0;
    }

    return 1;
}

void
permute(int numItem, char **data)
{
    int i, n, j;
    int *original = (int *) malloc(numItem * sizeof(int));
    int *working = (int *) malloc(numItem * sizeof(int));
    int *tmp = (int *) malloc(numItem * sizeof(int));
    int *stack[MAX_STACK];
    int stackPtr = 0;
    int *next;
    int wl = 0;

    for (i = 0; i < numItem; i++) {
        original[i] = i;
        working[i] = UNSET;
    }

    /* Push next on to the stack */
    next = (int *) malloc(numItem * sizeof(int));
    memcpy(next, original, sizeof(int) * numItem);
    stack[stackPtr++] = next;

    while (!isEmpty(next, numItem)) {
        working[wl++] = next[0];
        next = (int *) malloc(numItem * sizeof(int));
        memcpy(next, original, sizeof(int) * numItem);

        /* remove the working items from next */
        for (i = 0; i < numItem; i++) {
            for (j = 0; j < numItem; j++)
                if (next[i] == working[j])
                    next[i] = UNSET;
        }

        /*Pack the next array */
        n = 0;
        memset(tmp, UNSET, numItem * sizeof(int));
        for (i = 0; i < numItem; i++) {
            if (next[i] != UNSET)
                tmp[n++] = next[i];
        }
        memcpy(next, tmp, numItem * sizeof(int));

        /*if next is empty then print out the working list */
        if (isEmpty(next, numItem)) {
            for (i = 0; i < numItem; i++)
                printf("%s ", data[working[i]]);
            printf("\n");
        }

        /*backtrack */
        while (isEmpty(next, numItem) && stackPtr != 0) {
            free(next);
            next = stack[--stackPtr];

            /* remove the first item */
            n = 1;
            for (i = 0; i < numItem - 1; i++)
                next[i] = next[n++];
            next[numItem - 1] = UNSET;

            /* remove the last item */
            working[--wl] = UNSET;
        }
        stack[stackPtr++] = next;
    }
    while (stackPtr != 0)
        free(stack[--stackPtr]);
    free(working);
    free(original);
    free(tmp);
}

int
main(int argc, char **argv)
{
    argv++;
    permute(argc - 1, argv);
    return 0;
}
